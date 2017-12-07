using System;
using IBTradeRealTime.Position;
using NUnit.Framework;

namespace IBTradeRealTime.Test.Position
{
    internal class TestPositionMetricsBase
    {
        public static void AssertPositionMetrics(IPositionMetrics pnl, string position,
            string realized, string unrealized, string totalPnL)
        {
            Console.WriteLine(pnl.Position);
            AssertBigDecimal(position, pnl.Position);
            AssertBigDecimal(realized, pnl.RealizedPnL);
            AssertBigDecimal(unrealized, pnl.UnrealizedPnL);
            AssertBigDecimal(totalPnL, pnl.TotalPnL);
        }

        private static void AssertBigDecimal(string expected, decimal? actual)
        {
            if (expected == null)
                Assert.IsNull(actual);
            else
                Assert.AreEqual(Convert(expected), actual);
        }

        public static decimal? Convert(string strValue)
        {
            if (strValue == null)
                return null;
            return decimal.Parse(strValue);
        }
    }
}