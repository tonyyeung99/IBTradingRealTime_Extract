using IBTradeRealTime.Trade;

namespace IBTradeRealTime.Test.Position
{
    public class MockFutureInstrument : Instrument
    {
        private readonly string _symbol;

        public MockFutureInstrument(string symbol)
        {
            _symbol = symbol;
        }

        public override string GetSymbol()
        {
            return "FUT:" + _symbol;
        }

        public override SecurityType GetSecurityType()
        {
            return SecurityType.Future;
        }
    }
}