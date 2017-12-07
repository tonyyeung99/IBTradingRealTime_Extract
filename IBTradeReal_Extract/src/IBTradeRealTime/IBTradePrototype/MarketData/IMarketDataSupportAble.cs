namespace IBTradeRealTime.MarketData
{
    /**
    *Interface define the event handling functions need to be implemented in order to compatiable for IMarketDataSupport
    */
    public interface IMarketDataSupportAble
    {
        //handling funciton for receiving a new market data price
        void ClosePriceChanged(decimal tick);

        //handling funciton for receiving the trading price of a same instrument
        void SymbolTraded(decimal newPrice);
    }
}