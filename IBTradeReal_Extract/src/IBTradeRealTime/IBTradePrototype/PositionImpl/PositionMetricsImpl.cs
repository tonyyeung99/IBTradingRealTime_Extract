using IBTradeRealTime.Position;

namespace IBTradeRealTime.PositionImpl
{
    public class PositionMetricsImpl : IPositionMetrics
    {
        public PositionMetricsImpl()
        {
            Position = decimal.Zero;
            RealizedPnL = decimal.Zero;
        }

        public PositionMetricsImpl(decimal position, decimal realizedPnL, decimal? unrealizedPnL, decimal? totalPnL)
        {
            Position = position;
            RealizedPnL = realizedPnL;
            UnrealizedPnL = unrealizedPnL;
            TotalPnL = totalPnL;
        }

        public decimal Position { get; set; }

        public decimal RealizedPnL { get; set; }

        public decimal? UnrealizedPnL { get; set; }

        public decimal? TotalPnL { get; set; }
    }
}