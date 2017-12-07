using IBTradeRealTime.Trade;

namespace IBTradeRealTime.Position
{
    /**
    * Represents a row of position data. 
    */
    public interface IPositionRow
    {
        Instrument GetInstrument();

        string GetAccount();

        IPositionMetrics GetPositionMetrics();

        void SetPositionMetrics(IPositionMetrics positionMetrics);
    }
}