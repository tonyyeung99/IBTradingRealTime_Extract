namespace IBTradeRealTime.Position
{
    /**
    * Interface of the metrics of the position row, provide the get/set the different metrices reflecting the enconmic performance
    */
    public interface IPositionMetrics
    {
        decimal Position { get; set; }

        decimal RealizedPnL { get; set; }

        decimal? UnrealizedPnL { get; set; }

        decimal? TotalPnL { get; set; }
    }
}