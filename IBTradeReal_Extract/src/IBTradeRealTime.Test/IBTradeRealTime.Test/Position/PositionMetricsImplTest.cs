using IBTradeRealTime.PositionImpl;
using IBTradeRealTime.Test.Position;
using NUnit.Framework;

namespace IBTradeRealTime.Position
{
    [TestFixture]
    internal class PositionMetricsImplTest : TestPositionMetricsBase
    {
        public static IPositionMetrics CreateMetrics(string position, string realized, string unrealized,
            string totalPnL)
        {
            return new PositionMetricsImpl(Convert(position).Value, Convert(realized).Value, Convert(unrealized),
                Convert(totalPnL));
        }

        //Test the different constructor of PositionMetricsImpl work properly
        [Test]
        public void TestConstructor()
        {
            //Test Constructor without parameters
            AssertPositionMetrics(new PositionMetricsImpl(), position:"0", realized:"0", unrealized:null, totalPnL:null);

            //Test Constructor with null parameters
            AssertPositionMetrics(new PositionMetricsImpl(Convert("10").Value, Convert("100").Value, null, null), position: "10",
                realized:"100", unrealized:null, totalPnL:null);

            //Test Constructor with parameters(negative value)
            AssertPositionMetrics(
                new PositionMetricsImpl(Convert("-10").Value, Convert("100").Value, Convert("200"), Convert("300")),
               position:"-10", realized:"100", unrealized:"200", totalPnL:"300");

            //Test Constructor with parameters(decimal place value)
            AssertPositionMetrics(
                new PositionMetricsImpl(Convert("-10").Value, Convert("100.1").Value, Convert("200.2"),
                    Convert("300.3")),
                position:"-10", realized:"100.1", unrealized:"200.2", totalPnL:"300.3");
        }
    }
}