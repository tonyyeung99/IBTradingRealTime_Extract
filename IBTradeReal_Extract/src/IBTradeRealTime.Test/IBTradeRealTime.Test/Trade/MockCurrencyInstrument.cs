using IBTradeRealTime.Trade;

namespace IBTradeRealTime.Test.Position
{
    public class MockCurrencyInstrument : Instrument
    {
        private readonly string _currency1;
        private readonly string _currency2;

        public MockCurrencyInstrument(string currency1, string currency2)
        {
            _currency1 = currency1;
            _currency2 = currency2;
        }

        public override string GetSymbol()
        {
            return "FX:" + _currency1 + "/" + _currency2;
        }

        public override SecurityType GetSecurityType()
        {
            return SecurityType.Currency;
        }
    }
}