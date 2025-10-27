namespace TaskWork.Serwices
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.ML.Data;

    public class ArticlePrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedTags { get; set; }
    }

}
