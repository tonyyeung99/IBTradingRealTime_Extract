namespace IBTradeRealTime.Position
{
    /**
    * Interface of the row metrics calculator, define the type of calculation function
    */
    public interface IPositionMetricsCalculator
    {
        //Function to calculate the position row metrics by using a new trade price
        //(either a price of reciving a new market data, or a price of new traded price)
        IPositionMetrics Tick(decimal tradePrice);
        
        //Function to calculate the position row metrics by adding a new trade
        IPositionMetrics Trade(Trade.ITrade trade);
    }
}