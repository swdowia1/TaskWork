namespace TaskWork.FUN
{
    public class classFun
    {

        public static DateTime CurrentTimePoland()
        {
            TimeZoneInfo warsawTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime warsawTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, warsawTimeZone);
            return warsawTime;

        }

        public  static DateTime CurrentTimeUTC()
        {
            return DateTime.UtcNow;
        }
        internal static void opuznienie(int v)
        {
            Thread.Sleep(v * 1000);
        }
        public static string FormatMinutes(int minutes)
        {
           

            TimeSpan ts = TimeSpan.FromMinutes(minutes);

            // formatowanie do HH:MM
           return string.Format("{0:D2}H:{1:D2}", (int)ts.TotalHours, ts.Minutes);
        }
    }
}
