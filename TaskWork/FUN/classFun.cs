namespace TaskWork.FUN
{
    public class classFun
    {
      

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
