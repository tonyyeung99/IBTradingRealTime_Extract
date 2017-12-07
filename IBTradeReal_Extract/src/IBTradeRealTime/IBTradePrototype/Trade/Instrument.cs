namespace IBTradeRealTime.Trade
{
    /**
    * A financial instrument abstract class 
    */
    public abstract class Instrument
    {
        public abstract string GetSymbol();

        public abstract SecurityType GetSecurityType();

        public string GetFullSymbol()
        {
            return GetSymbol();
        }
    }
}