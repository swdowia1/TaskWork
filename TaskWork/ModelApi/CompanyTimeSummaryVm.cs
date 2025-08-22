namespace TaskWork.ModelApi
{
    public class CompanyTimeSummaryVm
    {
        public string CompanyName { get; set; } = string.Empty;
        public int CompanyId { get; set; }

        public string Today { get; set; } = "00:00";
        public string ThisWeek { get; set; } = "00:00";
        public string ThisMonth { get; set; } = "00:00";
        public string LastMonth { get; set; } = "00:00";
    }
}
