namespace TaskWork.Serwices
{
    using Microsoft.ML;
    using Microsoft.Data.SqlClient;
    using Microsoft.ML.Data;
    using System.Collections.Concurrent;

    // Surowy rekord artykułu z DB
    internal class RawArticle
    {
        public string Content { get; set; }
        public string Tags { get; set; } // "C#,ASP.NET" - CSV
    }

    // Klasa treningowa dla pojedynczego tagu
    internal class TagTrainData
    {
        public string Content { get; set; }
        public bool Label { get; set; } // true = ten tag występuje
    }

    // Predykcja prawdopodobieństwa (binary)
    internal class TagPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }

        [ColumnName("PredictedLabel")]
        public bool Predicted { get; set; }
    }

    public class TagPredictionService
    {
        private readonly IConfiguration _config;
        private readonly MLContext _mlContext;
        private readonly string _connectionString;

        // Model per-tag: przechowujemy ITransformer + PredictionEngine lub PredictionFunction
        private readonly ConcurrentDictionary<string, ITransformer> _models = new();
        private readonly ConcurrentDictionary<string, PredictionEngine<TagTrainData, TagPrediction>> _engines = new();

        // meta
        private static readonly object _trainLock = new();
        private DateTime _lastTrain = DateTime.MinValue;

        public TagPredictionService(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
            _mlContext = new MLContext(seed: 0);

            // Trenuj asynchronicznie w tle
            Task.Run(() => TrainAllModelsAsync());
        }

        /// <summary>
        /// Publiczne API: przewiduj listę tagów (multi-label) dla danej treści.
        /// </summary>
        public List<string> PredictTags(string content, double threshold = 0.5)
        {
            if (string.IsNullOrWhiteSpace(content))
                return new List<string>();

            // Jeśli modelów jeszcze nie ma, zwracamy pustą listę (lub można czekać/retrigować trening)
            if (_engines.IsEmpty)
                return new List<string>();

            var result = new List<string>();

            foreach (var kv in _engines)
            {
                try
                {
                    var engine = kv.Value;
                    var pred = engine.Predict(new TagTrainData { Content = content });
                    // Score może być log-odds; jednak w SDCA Logistic Score jest prawdopodobieństwem — sprawdź
                    if (pred.Score >= threshold)
                        result.Add(kv.Key);
                }
                catch
                {
                    // ignoruj błędy pojedynczych modeli
                }
            }

            return result;
        }

        /// <summary>
        /// Trenuje modele dla wszystkich istniejących tagów (w tle).
        /// </summary>
        public async Task TrainAllModelsAsync(bool force = false)
        {
            // Jednoczesne wywołanie blokujemy
            lock (_trainLock)
            {
                if (!force && (DateTime.Now - _lastTrain).TotalMinutes < 10 && _engines.Count > 0)
                    return; // już niedawno trenowano
                _lastTrain = DateTime.Now;
            }

            try
            {
                var raw = LoadRawArticles();
                if (!raw.Any())
                    return;

                // Zbierz listę unikalnych tagów z bazy
                var allTags = raw
                    .SelectMany(r => SplitTags(r.Tags))
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (!allTags.Any())
                    return;

                // Dla każdego taga zrób dataset i trenuj binary classifier
                foreach (var tag in allTags)
                {
                    // przygotuj dane treningowe: Label = czy tag jest w Tags CSV
                    var trainList = raw.Select(r => new TagTrainData
                    {
                        Content = r.Content,
                        Label = SplitTags(r.Tags).Any(t => string.Equals(t.Trim(), tag, StringComparison.OrdinalIgnoreCase))
                    })
                    .Where(t => !string.IsNullOrWhiteSpace(t.Content))
                    .ToList();

                    // Musimy mieć zarówno pozytywne jak i negatywne próbki
                    var positiveCount = trainList.Count(x => x.Label);
                    var negativeCount = trainList.Count - positiveCount;
                    if (positiveCount < 3 || negativeCount < 3)
                    {
                        // brak próbek wystarczająco; pomiń lub można dopasować próg
                        continue;
                    }

                    var dataView = _mlContext.Data.LoadFromEnumerable(trainList);

                    // pipeline: text featurize -> sdca logistic (binary)
                    var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(TagTrainData.Content))
                        .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: nameof(TagTrainData.Label), featureColumnName: "Features"));

                    var model = pipeline.Fit(dataView);

                    // prediction engine — thread-unsafe, ale przechowamy i użyjemy na pojedynczym wątku;
                    // dla wielowątkowości lepiej użyć PredictionEnginePool lub ITransformer.Transform + CreateEnumerable
                    var engine = _mlContext.Model.CreatePredictionEngine<TagTrainData, TagPrediction>(model);

                    // zapisujemy do cache (nadpisujemy istniejący)
                    _models.AddOrUpdate(tag, model, (k, v) => model);
                    _engines.AddOrUpdate(tag, engine, (k, v) => engine);
                }
            }
            catch (Exception ex)
            {
                // loguj błąd
                Console.WriteLine("[ML ERROR] TrainAllModelsAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Ładuje artykuły z bazy (content + tags CSV)
        /// </summary>
        private List<RawArticle> LoadRawArticles()
        {
            var list = new List<RawArticle>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using var cmd = new SqlCommand(@"
                    SELECT a.Content,
                           STUFF((
                               SELECT ',' + t.Name
                               FROM ArticleTags at
                               INNER JOIN Tags t ON at.TagId = t.Id
                               WHERE at.ArticleId = a.Id
                               FOR XML PATH(''), TYPE
                           ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS Tags
                    FROM Articles a
                ", conn);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var content = reader["Content"]?.ToString() ?? "";
                    var tags = reader["Tags"]?.ToString() ?? "";
                    if (!string.IsNullOrWhiteSpace(content))
                        list.Add(new RawArticle { Content = content, Tags = tags });
                }
            }

            return list;
        }

        private static IEnumerable<string> SplitTags(string csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) yield break;
            foreach (var p in csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                yield return p.Trim();
        }
    } 

}
