using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class TradeAction
{
    public enum Action
    {
        Long,
        Short,
    }

    public TradeAction(Action action, string symbol, int quantity, ICandle candle)
    {
        this.OrderAction = action;
        Symbol = symbol;
        Quantity = quantity;
        Candle = candle;
    }

    public Action OrderAction { get; set; }
    public string Symbol { get; private set; }
    public int Quantity { get; private set; }
    public ICandle Candle { get; private set; }
}
