namespace TaskWork.Serwices
{
    using Microsoft.ML;

    public class TagPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<ArticleData, ArticlePrediction> _predictionEngine;

        public TagPredictionService()
        {
            _mlContext = new MLContext();

            // Wczytaj wytrenowany model (np. z folderu projektu)
            if (File.Exists("articleTagModel.zip"))
            {
                _model = _mlContext.Model.Load("articleTagModel.zip", out var schema);
                _predictionEngine = _mlContext.Model.CreatePredictionEngine<ArticleData, ArticlePrediction>(_model);
            }
        }

        public List<string> PredictTags(string content)
        {
            if (_predictionEngine == null)
                return new List<string>();

            var prediction = _predictionEngine.Predict(new ArticleData { Content = content });
            return prediction.PredictedTags.Split(',').Select(t => t.Trim()).ToList();
        }
    }

}
