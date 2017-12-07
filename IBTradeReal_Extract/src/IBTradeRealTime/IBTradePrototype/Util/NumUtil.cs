namespace IBTradeRealTime.util
{
    public class NumberUtil
    {
        public static int SignNum(decimal? number)
        {
            if (number == 0)
                return 0;
            return (number > 0) ? 1 : -1;
        }
    }
}