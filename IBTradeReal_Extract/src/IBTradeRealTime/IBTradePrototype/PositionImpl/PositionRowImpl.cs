using IBTradeRealTime.Position;
using IBTradeRealTime.Trade;

namespace IBTradeRealTime.PositionImpl
{
    public class PositionRowImpl : IPositionRow
    {
        private readonly string _account;
        private readonly Instrument _instrument;
        private volatile IPositionMetrics _positionMetrics;

        public PositionRowImpl(Instrument instrument, string account) : this(instrument, account, null)
        {
        }

        public PositionRowImpl(Instrument instrument, string account, IPositionMetrics metrics)
        {
            _instrument = instrument;
            _account = account;
            _positionMetrics = metrics;
        }


        public Instrument GetInstrument()
        {
            return _instrument;
        }

        public string GetAccount()
        {
            return _account;
        }


        public IPositionMetrics GetPositionMetrics()
        {
            return _positionMetrics;
        }


        public void SetPositionMetrics(IPositionMetrics positionMetrics)
        {
            _positionMetrics = positionMetrics;
        }
    }
}