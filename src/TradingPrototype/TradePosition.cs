using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class TradePosition
{
    public DateTime OpenTime { get; private set; }
    public TimeSpan OpenDuration { get; private set; }

    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public decimal GainLoss { get; private set; }
    public decimal GainLossPercent { get; private set; }
    public decimal TotalCost => OpenCandle.Close * Quantity;
    public decimal TotalValue=> CloseCandle.Close * Quantity;

    public string Symbol { get; private set; }

    public ICandle CloseCandle { get; private set; }
    public ICandle OpenCandle { get; private set; }

    public TradePosition(TradeAction action, ICandle currentCandle)
    {
        this.OpenCandle = action.Candle;
        this.CloseCandle = currentCandle;

        OpenTime = action.Candle.Timestamp;
        OpenDuration = OpenTime - currentCandle.Timestamp;

        Price = action.Candle.Close;
        Quantity = action.OrderAction == TradeAction.Action.Long ? action.Quantity : -action.Quantity;
        Symbol = action.Symbol;
        GainLoss = TotalValue - TotalCost;
        GainLossPercent = GainLoss / Math.Abs(TotalCost); 
    }

}
