using System.Threading;
using IBTradeRealTime.Position;
using IBTradeRealTime.PositionImpl;
using NUnit.Framework;

namespace IBTradeRealTime.Test.Position
{
    [TestFixture]
    internal class TestPositionRowUpdater : TestPositionMetricsBase
    {
        [SetUp]
        public void Init()
        {
            _futureInstrument = new MockFutureInstrument("HSI16J");
            _futureInstrumentSymbol2 = new MockFutureInstrument("HSI16K");
            _positionRow = new PositionRowImpl(_futureInstrument, "Account1");
            _positionRowWithAccount2 = new PositionRowImpl(_futureInstrument, "Account2");
            _positionRowWithSymbol2 = new PositionRowImpl(_futureInstrumentSymbol2, "Account1");
            _futureMarketData = new MockFutureMarketData(_futureInstrument);
            _futureMarketDataWithSymbol2 = new MockFutureMarketData(_futureInstrumentSymbol2);
            _rowUpdater = new PositionRowUpdater(_positionRow, _futureMarketData);
            _rowUpdaterWithAccount2 = new PositionRowUpdater(_positionRowWithAccount2, _futureMarketData);
            _rowUpdaterWithSymbol2 = new PositionRowUpdater(_positionRowWithSymbol2, _futureMarketDataWithSymbol2);
        }

        private MockFutureInstrument _futureInstrument;
        private MockFutureInstrument _futureInstrumentSymbol2;
        private IPositionRow _positionRow;
        private IPositionRow _positionRowWithAccount2;
        private IPositionRow _positionRowWithSymbol2;
        private PositionRowUpdater _rowUpdater;
        private PositionRowUpdater _rowUpdaterWithAccount2;
        private PositionRowUpdater _rowUpdaterWithSymbol2;
        private MockFutureMarketData _futureMarketData;
        private MockFutureMarketData _futureMarketDataWithSymbol2;

        //Given: Two position row with some holdings: both position row has the same account but with different symbol
        //When: Latest close price event the two different symbol trigger 
        //Then: The unrealized PnL of two positon calculate correctly
        [Test]
        public void TestUpdateAddNewTradesWithMultipleSymbol()
        {
            //[Position Row 1 : Account1, Symbol1]
            //Add 1st trade with quantity = 100, price = $5
            //Then Add 2nd trade with quantity = -50, price = $6 
            UpdateTradesNotFullyOffsetEachOther(_rowUpdater, _futureInstrument, "100", "5", "-50", "6");
            var metrics = _rowUpdater.GetPosition().GetPositionMetrics();
            //Expected the Unrealized PnL =$50
            AssertPositionMetrics(metrics, "50", "50", "50", "100");

            //[Position Row 2 : Account1, Symbol2]
            //Add 1st trade with quantity = -100, price = $6
            //Then Add 2nd trade with quantity = 50, price = $9 
            UpdateTradesNotFullyOffsetEachOther(_rowUpdaterWithSymbol2, _futureInstrumentSymbol2, "-100", "6", "50",
                "9");
            metrics = _rowUpdaterWithSymbol2.GetPosition().GetPositionMetrics();
            //Expected the Unrealized PnL =$50
            AssertPositionMetrics(metrics, "-50", "-150", "-150", "-300");


            //Provide a close price with $7 (Symbol 1)
            TriggerNewClosePriceEvent(_futureMarketData, "7");
            //Provide a close price with $10 (Symbol 2)
            TriggerNewClosePriceEvent(_futureMarketDataWithSymbol2, "10");


            //Expected the Unrealized of the Position Row 1 changed from $50 to $100
            metrics = _rowUpdater.GetPosition().GetPositionMetrics();
            AssertPositionMetrics(metrics, "50", "50", "100", "150");

            //Expected the Unrealized of the Position Row 1 changed from $-150 to -$200
            metrics = _rowUpdaterWithSymbol2.GetPosition().GetPositionMetrics();
            AssertPositionMetrics(metrics, "-50", "-150", "-200", "-350");
        }

        //Test update the latest close price, and the unrealized PnL calculate correctly;
        //Given: One position row with some holdings
        //When: Latest close price event trigger 
        //Then: The unrealized PnL of two positon calculate correctly
        [Test]
        public void TestUpdateLatestClosePrice()
        {
            //Add 1st trade with quantity = 100, price = $5
            //Then Add 2nd trade with quantity = -50, price = $6 
            UpdateTradesNotFullyOffsetEachOther(_rowUpdater, _futureInstrument, "100", "5", "-50", "6");
            var metrics = _rowUpdater.GetPosition().GetPositionMetrics();
            //Expected the Unrealized PnL =$50
            AssertPositionMetrics(metrics, "50", "50", "50", "100");

            //Provide a close price with $4
            TriggerNewClosePriceEvent(_futureMarketData, "4");
            Thread.Sleep(1000);
            metrics = _rowUpdater.GetPosition().GetPositionMetrics();
            //Expected the Unrealized PnL changed from $50 to $-50
            AssertPositionMetrics(metrics, "50", "50", "-50", "0");
        }

        //Given: two position rows with some holdings: both position row has the same symbol but different accounts
        //When: latest close price event the same symbol trigger 
        //Then: The unrealized PnL of two positon calculate correctly
        [Test]
        public void TestUpdateMultipleAccountsCommonSymbol()
        {
            //[Position Row 1 : Account1, Symbol1]
            //Add 1st trade with quantity = 100, price = $5
            //Then Add 2nd trade with quantity = -50, price = $6 
            UpdateTradesNotFullyOffsetEachOther(_rowUpdater, _futureInstrument, "100", "5", "-50", "6");
            var metrics = _rowUpdater.GetPosition().GetPositionMetrics();
            //Expected the Unrealized PnL =$50
            AssertPositionMetrics(metrics, "50", "50", "50", "100");

            //[Position Row 2 : Account2, Symbol1]
            //Add 1st trade with quantity = -100, price = $6
            //Then Add 2nd trade with quantity = 50, price = $8 
            UpdateTradesNotFullyOffsetEachOther(_rowUpdaterWithAccount2, _futureInstrument, "-100", "6", "50", "8");
            metrics = _rowUpdaterWithAccount2.GetPosition().GetPositionMetrics();
            //Expected the Unrealized PnL =$50
            AssertPositionMetrics(metrics, "-50", "-100", "-100", "-200");

            //Provide a close price with $4 for Symbol 1
            TriggerNewClosePriceEvent(_futureMarketData, "7");
            Thread.Sleep(1000);

            //Expected the Unrealized of the Position Row 1 changed from $50 to $100
            metrics = _rowUpdater.GetPosition().GetPositionMetrics();
            AssertPositionMetrics(metrics, "50", "50", "100", "150");

            //Expected the Unrealized of the Position Row 1 changed from $-100 to -$50
            metrics = _rowUpdaterWithAccount2.GetPosition().GetPositionMetrics();
            AssertPositionMetrics(metrics, "-50", "-100", "-50", "-150");
        }

        private MockFutureTrade createMockFutureTrade(MockFutureInstrument futureInstrument, string quantity,
            string price)
        {
            return new MockFutureTrade(futureInstrument, Convert(quantity).Value, Convert(price).Value);
        }

        private void TriggerNewClosePriceEvent(MockFutureMarketData marketData, string price)
        {
            marketData.FireClosePriceEvent(Convert(price).Value);
        }

        private void UpdateTradesNotFullyOffsetEachOther(PositionRowUpdater updater, MockFutureInstrument instrument,
            string qty1, string price1, string qty2,
            string price2)
        {
            //Todo: check the sign qty2 and qty1 is opposite and qty2 not equal to qty1 
            updater.UpdateTrade(createMockFutureTrade(instrument, qty1, price1));
            updater.UpdateTrade(createMockFutureTrade(instrument, qty2, price2));
        }
    }
}