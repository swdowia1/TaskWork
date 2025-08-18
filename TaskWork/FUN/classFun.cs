namespace TaskWork.FUN
{
    public class classFun
    {
        public static string FormatMinutes(int total)
        {
            var ts = TimeSpan.FromMinutes(total);
            return $"{(int)ts.TotalHours:00}:{ts.Minutes:00}";
        }

        internal static void opuznienie(int v)
        {
            Thread.Sleep(v * 1000);
        }
    }
}
