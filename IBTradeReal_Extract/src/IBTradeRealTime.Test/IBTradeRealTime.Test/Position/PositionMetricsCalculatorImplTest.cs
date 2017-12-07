using IBTradeRealTime.Position;
using IBTradeRealTime.PositionImpl;
using NUnit.Framework;

namespace IBTradeRealTime.Test.Position
{
    [TestFixture]
    internal class PositionMetricsCalculatorImplTest : TestPositionMetricsBase
    {
        [SetUp]
        public void Init()
        {
            _calculator = new PositionMetricsCalculatorImpl();
            _futureInstrument = new MockFutureInstrument("HSI16J");
        }

        private IPositionMetricsCalculator _calculator;
        private MockFutureInstrument _futureInstrument;

        //Given: A position row with no holdings
        //When: Add a long trade 
        //Then: Expect the positive number of share reflected, and all PnL are zero
        [Test]
        public void TestCalculationAfterOneLongTrade()
        {
            var metrics = _calculator.Trade(CreateMockFutureTrade(_futureInstrument, "100", "3.2"));
            AssertMetricsWithNonZeroPosition(metrics, "100", "0", "0", "0");
        }

        //Given: A position row with no holdings
        //When: Add a long trade 
        //Then: Expect the negative number of share reflected, and all PnL are zero
        [Test]
        public void TestCalculationAfterOneShortTrade()
        {
            var metrics = _calculator.Trade(CreateMockFutureTrade(_futureInstrument, "-100", "3.2"));
            AssertMetricsWithNonZeroPosition(metrics, "-100", "0", "0", "0");
        }

        //Given: A position row with no holdings
        //When: Add/Remove trades, open/close the position serveral times     
        //Then: Expect the realized PnL calculate/accumulate correctly
        [Test]
        public void TestCalculationAfterOpenClosePositionMultipleTimes()
        {
            //Open and close position with a realized PnL = -100  (First Time)
            var metrics = CalMetricsAfterTradesOffsetEachOther("100", "4", "-100", "3");
            AssertMetricsWithZeroPosition(metrics, "-100", "0", "-100");

            //Open and close position with a ralized Pnl = 400 (Second Time, total realized PnL = 400 -100 = 300)
            metrics = CalMetricsAfterTradesOffsetEachOther("200", "5", "-200", "7");
            AssertMetricsWithZeroPosition(metrics, "300", "0", "300");
        }

        //Test calculate metrics after add multiple trades with different prices 
        //Test able to calculatee the internal average price correctly
        //Given: A position row with no holdings
        //When: Add two long trades with different prices, add another short trade to offset the position later    
        //Then: Expect the internal cost and the realized PnL calculate/accumulate correctly
        [Test]
        public void TestCalculationAverageCostAfterTradesWithDifferentPrices()
        {
            //Add 2 long trades 
            //The internal cost of the holdings should be $4 per share
            var metrics = CalMetricsAfterTradesWithDifferentPrices("100", "3", "100", "5");

            //Close the position with a long trade with the price of $2 per share, expect losing total $400 in realized PnL 
            metrics = CalMetricsAfterOffsetExistingHolding("-200", "2");
            AssertMetricsWithZeroPosition(metrics, "-400", "0", "-400");
        }

        //Test calculate metrics after close position with remain holding
        //Test able to calculatee the internal average price correctly
        //Given: A position row with no holdings
        //When: Add a long trade, add a short trade with quantity less than the long trade    
        //Then: Expect the realized PnL and unrealized PnL calculate/accumulate correctly
        [Test]
        public void TestCalculationClosePositionWithRemain()
        {
            var metrics = CalMetricsAfterTradesNotFullyOffsetEachOther("100", "3.2", "-50", "4");
            AssertMetricsWithNonZeroPosition(metrics, "50", "40", "40", "80");
        }

        //Test calculate metrics after no trades and with last price provided
        //Given: A position row with no holdings
        //When: Provide a close price to the calculator 
        //Then: Expect no error raised in the calculation and all the metrics are zero
        [Test]
        public void TestNoTradesButWithLastPriceProvided()
        {
            var metrics = _calculator.Tick(Convert("6").Value);
            AssertMetricsWithNonZeroPosition(metrics, "0", "0", "0", "0");
        }

        private IPositionMetrics CalMetricsAfterTradesNotFullyOffsetEachOther(string qty1, string price1, string qty2,
            string price2)
        {
            //Todo: check the sign qty2 and qty1 is opposite and qty2 not equal to qty1
            _calculator.Trade(CreateMockFutureTrade(_futureInstrument, qty1, price1));
            return _calculator.Trade(CreateMockFutureTrade(_futureInstrument, qty2, price2));
        }


        private IPositionMetrics CalMetricsAfterTradesOffsetEachOther(string qty1, string price1, string qty2,
            string price2)
        {
            //Todo: check qty1 offset qty2
            _calculator.Trade(CreateMockFutureTrade(_futureInstrument, qty1, price1));
            return _calculator.Trade(CreateMockFutureTrade(_futureInstrument, qty2, price2));
        }

        private IPositionMetrics CalMetricsAfterTradesWithDifferentPrices(string qty1, string price1, string qty2,
            string price2)
        {
            //Todo: check price1 different from price2
            _calculator.Trade(CreateMockFutureTrade(_futureInstrument, qty1, price1));
            return _calculator.Trade(CreateMockFutureTrade(_futureInstrument, qty2, price2));
        }

        private IPositionMetrics CalMetricsAfterOffsetExistingHolding(string qty, string price)
        {
            //Todo: check qty can offset existing hodlings
            return _calculator.Trade(CreateMockFutureTrade(_futureInstrument, qty, price));
        }

        private void AssertMetricsWithZeroPosition(IPositionMetrics metrics, string realizedPnL, string unrealizedPnL,
            string totalPnL)
        {
            AssertPositionMetrics(metrics, "0", realizedPnL, unrealizedPnL, totalPnL);
        }

        private void AssertMetricsWithNonZeroPosition(IPositionMetrics metrics, string position, string realizedPnL,
            string unrealizedPnL, string totalPnL)
        {
            AssertPositionMetrics(metrics, position, realizedPnL, unrealizedPnL, totalPnL);
        }

        private static MockFutureTrade CreateMockFutureTrade(MockFutureInstrument futureInstrument, string quantity,
            string price)
        {
            return new MockFutureTrade(futureInstrument, Convert(quantity).Value, Convert(price).Value);
        }


    }
}