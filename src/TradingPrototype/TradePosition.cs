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

    public float Cost { get; private set; }
    public int Quantity { get; private set; }
    public float GainLoss { get; private set; }
    public float GainLossPercent { get; private set; }
    public float TotalCost => Cost * Quantity;
    public string Symbol { get; private set; }

    public TradePosition(TradeAction action, Candle currentCandle)
    {
        OpenTime = action.Candle.Timestamp;
        OpenDuration = OpenTime - currentCandle.Timestamp;

        Cost = action.Candle.Close;
        Quantity = action.Quantity;
        Symbol = action.Symbol;
        GainLoss = currentCandle.Close * Quantity - TotalCost;
        GainLossPercent = GainLoss / TotalCost; 
    }

}
