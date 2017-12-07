namespace IBTradeRealTime.Trade
{
    public interface ITrade
    {
        /**
         * An Interface for Trade object
         */
        Instrument GetInstrument();

        decimal GetPrice();

        decimal GetQuantity();

        long GetSequenceNumber();
    }
}