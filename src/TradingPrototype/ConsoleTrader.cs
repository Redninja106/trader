﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class ConsoleTrader : ITrader
{
    public ConsoleTrader(IStrategy strategy)
    {
        Strategy = strategy;
        ClosedTrades = new();
    }

    public List<TradePosition> ClosedTrades { get; }
    public IStrategy Strategy { get; }

    private TradeAction? position = null;

    public void Pump(Dictionary<string, Candle> candles)
    {
        if (position is null)
        {
            if (Strategy.ShouldEnter(out var action))
            {
                position = action;
                Console.WriteLine($"Entered on {action.Candle.Timestamp} at ${action.Candle.Close}");
            }
        }
        else
        {
            var currentCandle = candles[position.Symbol];
            var pos = new TradePosition(this.position, currentCandle);
            if (Strategy.ShouldExit(pos, out var action))
            {
                ClosedTrades.Add(pos);
                Console.WriteLine($"Exited on {currentCandle.Timestamp} with a net change of ${pos.GainLoss} ({pos.GainLossPercent*100}%)");
                this.position = null;
            }
        }
    }
}