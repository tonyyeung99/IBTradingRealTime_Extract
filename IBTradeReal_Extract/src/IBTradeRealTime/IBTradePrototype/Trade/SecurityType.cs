using System;

namespace IBTradeRealTime.Trade
{
    [Flags]
    public enum SecurityType
    {
        Unknown = 0,
        CommonStock = 1,
        Option = 2,
        Future = 3,
        Currency = 4
    }
}