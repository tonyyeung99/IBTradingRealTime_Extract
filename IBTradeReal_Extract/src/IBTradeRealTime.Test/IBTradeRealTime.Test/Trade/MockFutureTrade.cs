using IBTradeRealTime.Trade;

namespace IBTradeRealTime.Test.Position
{
    internal class MockFutureTrade : ITrade
    {
        private readonly decimal _price;
        private readonly decimal _quantity;
        private readonly Instrument _instruement;


        public MockFutureTrade(Instrument instruement, decimal quantity, decimal price)
        {
            _quantity = quantity;
            _price = price;
            _instruement = instruement;
        }

        public decimal GetPrice()
        {
            return _price;
        }

        public decimal GetQuantity()
        {
            return _quantity;
        }

        public long GetSequenceNumber()
        {
            return 0;
        }

        public Instrument GetInstrument()
        {
            return _instruement;
        }
    }
}