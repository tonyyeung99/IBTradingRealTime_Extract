using System;
using IBTradeRealTime.Position;
using IBTradeRealTime.util;

namespace IBTradeRealTime.PositionImpl
{
    /**
    * A position row calculator class contain both the state variablie and functions for metrics calcuation
    */
    public class PositionMetricsCalculatorImpl : IPositionMetricsCalculator
    {
        private readonly CostElement _positionCost = new CostElement();//An state object used to calculate the cost of the holding position
        //private bool _closingPriceAvailable; //
        private decimal? _lastAvailablePrice; //Last reciving market data price or last trade price of the symbol trade
        private decimal _position = decimal.Zero;//number of shares: +ve for long positions and -ve for short positions
        private decimal _realizedPl = decimal.Zero;//+ve for profit, -ve for loss

        IPositionMetrics IPositionMetricsCalculator.Tick(decimal tradePrice)
        {
            _lastAvailablePrice = tradePrice;
            //_closingPriceAvailable = true;
            return CreatePositionMetrics();
        }

        IPositionMetrics IPositionMetricsCalculator.Trade(Trade.ITrade trade)
        {
            ProcessTrade(trade.GetQuantity(), trade.GetPrice());
            return CreatePositionMetrics();
        }

        //Proccess a new trade
        private void ProcessTrade(decimal quantity, decimal price)
        {
            var holdingSide = NumberUtil.SignNum(_position);
            var tradeSide = NumberUtil.SignNum(quantity);
            _lastAvailablePrice = price;
            if (holdingSide * tradeSide == -1)
                ProcessClose(quantity, _positionCost.GetAverageCost(), price);
            else
                _positionCost.Add(quantity, price);
            _position = _position + quantity;
        }

        //If the new trade offseting to the holding positions, execute this function
        private void ProcessClose(decimal quantity, decimal openPrice, decimal closePrice)
        {
            _realizedPl = _realizedPl + quantity * (openPrice - closePrice);
            _positionCost.Remove(-quantity);
        }


        //Return a view on the positing metrics
        private IPositionMetrics CreatePositionMetrics()
        {
            decimal? unrealizedPl = null;
            decimal? totalPl = null;
           // if (_closingPriceAvailable)
                if (_lastAvailablePrice.HasValue)
                {
                    unrealizedPl = _position * (_lastAvailablePrice - _positionCost.GetAverageCost());
                    totalPl = unrealizedPl + _realizedPl;
                }
            var positionMetrics = new PositionMetricsImpl(_position, _realizedPl, unrealizedPl, totalPl);
            return positionMetrics;
        }

        private class CostElement
        {
            private decimal _cost = decimal.Zero;
            private decimal _quantity = decimal.Zero;

            public void Add(decimal quantity, decimal price)
            {
                _quantity = _quantity + quantity;
                _cost = _cost + quantity * price;
            }

            public void Remove(decimal newQuantity)
            {
                var origPerCost = _cost / _quantity;
                _quantity = _quantity - newQuantity;
                _cost = _cost - origPerCost * newQuantity;
            }

            public decimal GetPnL(decimal lastTradePrice)
            {
                return _quantity * lastTradePrice - _cost;
            }

            public decimal GetCost()
            {
                return _cost;
            }

            public decimal GetQuantity()
            {
                return _quantity;
            }

            public decimal GetAverageCost()
            {
                if (GetQuantity() == 0)
                    return decimal.Zero;
                return decimal.Divide(GetCost(), GetQuantity());
            }
        }
    }
}