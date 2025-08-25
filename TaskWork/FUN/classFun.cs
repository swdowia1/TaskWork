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
            return $"{minutes / 60:D2}:{minutes % 60:D2}";
        }
    }
}
