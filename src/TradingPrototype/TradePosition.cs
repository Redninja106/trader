using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Options;

namespace TradingPrototype;
internal class TradePosition
{
    public DateTime OpenTime { get; private set; }
    public TimeSpan OpenDuration { get; private set; }

    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public decimal GainLoss { get; private set; }
    public decimal GainLossPercent { get; private set; }
    public decimal TotalCost { get; private set; }
    public decimal TotalValue { get; private set; }

    public string Symbol { get; private set; }

    //public ICandle CloseCandle { get; private set; }

    public IOption Option { get; private set; }

    public TradePosition(DateTime asOf, TradeAction action, ICandle currentCandle)
    {
        OpenTime = action.TradeDate;
        OpenDuration = OpenTime - asOf;

        int qty = action.OrderAction == TradeAction.Action.Long ? action.Quantity : -action.Quantity;

        var shareCount = action.Quantity;
        var openCost = (decimal)action.Price;
        var closeCost = (decimal)currentCandle.Close;

        this.TotalCost = (shareCount * openCost) * (action.OrderAction == TradeAction.Action.Long ? 1 : -1);
        this.TotalValue = (shareCount * closeCost) * (action.OrderAction == TradeAction.Action.Long ? 1 : -1);

        var gainLoss = this.TotalValue - this.TotalCost;

        Price = action.Price;
        Quantity = qty;
        Symbol = action.Option != null ? action.Option.Symbol : action.Symbol.Symbol;
        GainLoss = gainLoss;
        GainLossPercent = gainLoss / Math.Abs(this.TotalCost);
    }

    public TradePosition(DateTime asOf, TradeAction action, IOption option)
    {
        this.Option = option; 
        OpenTime = action.TradeDate;
        OpenDuration = OpenTime - asOf;

        int qty = action.OrderAction == TradeAction.Action.Long ? action.Quantity : -action.Quantity;

        var shareCount = action.Quantity * 100;
        var openCost = (decimal)action.Option.Price;
        var closeCost = (decimal)option.Price;

        this.TotalCost = shareCount * openCost * (action.OrderAction == TradeAction.Action.Long ? 1 : -1);
        this.TotalValue = (shareCount * closeCost) * (action.OrderAction == TradeAction.Action.Long ? 1 : -1) ;

        var gainLoss = this.TotalValue - this.TotalCost;

        Price = action.Price;
        Quantity = qty;
        Symbol = action.Option != null ? action.Option.Symbol : action.Symbol.Symbol;
        GainLoss = gainLoss;
        GainLossPercent = gainLoss / Math.Abs(this.TotalCost); 
    }

}
