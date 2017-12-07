using IBTradeRealTime.Trade;

namespace IBTradeRealTime.MarketData
{
    /**
    *Interface of the MarketData Event subcription 
    *This interface define the capability to add the listner functions of the IMarketDataSupportAble to subscribtion of market data event
    */
    public interface IMarketDataSupport
    {
        void AddInstrumentMarketDataListener(Instrument instrument,
            IMarketDataSupportAble listenerClass);

        Instrument GetInstrument();
    }
}