namespace TaskWork.ModelApi
{
    public class TaskSummaryDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<TimeEntryDto> TimeEntries { get; set; } = new();
        public int TotalMinutes { get; set; }
        public double TotalHours => TotalMinutes / 60.0;
    }
}
