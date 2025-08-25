namespace TaskWork.ModelApi
{
    public class DateRange
    {
        public DateTime Day { get; set; }
        public DateTime Week { get; set; }
        public DateTime Month { get; set; }
        public DateTime MonthPrev { get; set; }
        public DateRange()
        {
            DateTime now = DateTime.UtcNow;

            // Początek dnia (UTC)
            Day = now.Date;

            // Początek tygodnia (np. poniedziałek jako pierwszy dzień tygodnia)
            int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
            Week = now.Date.AddDays(-diff);

            // Początek bieżącego miesiąca
            Month = new DateTime(now.Year, now.Month, 1);

            // Początek poprzedniego miesiąca
            MonthPrev = Month.AddMonths(-1);
        }

        public DateTime DateFrom(string typ)
        {
            if(typ=="d")
                return Day;
            if (typ == "w")  
                return Week;
            if (typ == "m")
                return Month;
            return MonthPrev;
        }
    }
}
