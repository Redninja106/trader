using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class TradeAction
{
    public TradeAction(string symbol, int quantity, Candle candle)
    {
        Symbol = symbol;
        Quantity = quantity;
        Candle = candle;
    }

    public string Symbol { get; private set; }
    public int Quantity { get; private set; }
    public Candle Candle { get; private set; }
}
