using System.Diagnostics;
using IBTradeRealTime.MarketData;
using IBTradeRealTime.Trade;

namespace IBTradeRealTime.Test.Position
{
    public class MockFutureMarketData : IMarketDataSupport
    {
        public delegate void ClosePriceChangedHandler(decimal price);

        private Instrument _instrument;

        public MockFutureMarketData(Instrument instrument)
        {
            _instrument = instrument;
        }

        public void AddInstrumentMarketDataListener(Instrument instrument, IMarketDataSupportAble listenerClass)
        {
            ClosePriceEvent += listenerClass.ClosePriceChanged;
            SymbolTradedEvent += listenerClass.SymbolTraded;
        }

        private event ClosePriceChangedHandler ClosePriceEvent;

        private event SymbolTraded SymbolTradedEvent;

        public void FireClosePriceEvent(decimal closePrice)
        {
            Debug.Assert(ClosePriceEvent != null, "ClosePriceEvent != null");
            ClosePriceEvent(closePrice);
        }

        public void FireSymbolTraded(decimal closePrice)
        {
            if (SymbolTradedEvent != null) SymbolTradedEvent(closePrice);
        }

        private delegate void SymbolTraded(decimal price);


        public Instrument GetInstrument()
        {
            return _instrument;
        }
    }
}