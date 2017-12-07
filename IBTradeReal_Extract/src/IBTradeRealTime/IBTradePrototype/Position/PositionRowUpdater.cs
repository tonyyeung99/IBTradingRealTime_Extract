using IBTradeRealTime.MarketData;
using IBTradeRealTime.PositionImpl;
using IBTradeRealTime.Trade;

namespace IBTradeRealTime.Position
{
    /**
    *An update adapter class between the market data source and the PositiionMetricsCalculator
    *-It defined the handling functions for the specified market data event 
    *-The handling functions define which Calculator function called
    */
    public class PositionRowUpdater : IMarketDataSupportAble
    {
        private readonly IPositionRow _positionRow;
        private volatile IPositionMetricsCalculator _calculator;

        public PositionRowUpdater(IPositionRow positionRow, IMarketDataSupport marketData
        )
        {
            marketData.AddInstrumentMarketDataListener(positionRow.GetInstrument(), this);
            _positionRow = positionRow;
            _positionRow.SetPositionMetrics(new PositionMetricsImpl());
            _calculator = new PositionMetricsCalculatorImpl();
        }

        //handling funciton for receiving a new market data price
        void IMarketDataSupportAble.ClosePriceChanged(decimal tick)
        {
            Tick(tick);
        }

        //handling funciton for receiving the trading price of a same instrument
        public void SymbolTraded(decimal newPrice)
        {
            Tick(newPrice);
        }

        public IPositionRow GetPosition()
        {
            return _positionRow;
        }

        private void Tick(decimal tick)
        {
            _positionRow.SetPositionMetrics(_calculator.Tick(tick));
        }

        public void UpdateTrade(ITrade trade)
        {
            _positionRow.SetPositionMetrics(_calculator.Trade(trade));
        }
    }
}