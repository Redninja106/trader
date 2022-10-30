using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Options;

namespace TradingPrototype;
internal class ConsoleTrader : ITrader
{
    public ConsoleTrader(IStrategy strategy)
    {
        Strategy = strategy;
        Transactions = new();
    }

    public List<(TradeAction OpenTrade, TradeAction CloseTrade, TradePosition Position)> Transactions { get; }
    public IStrategy Strategy { get; }

    private TradeAction? openAction = null;

    public void Pump(Dictionary<string, ICandle> candles)
    {
        if (openAction is null)
        {
            if (Strategy.ShouldEnter(out var action))
            {
                openAction = action;
                Console.WriteLine($"Entered on {action.TradeDate} at ${action.Price:c}");
            }
        }
        else
        {
            var currentCandle = candles[openAction.Option != null ? openAction.Option.UnderlyingSymbol.Symbol : openAction.Symbol.Symbol];
            TradePosition pos;

            IOption option = null;
            if(this.openAction.Option != null)
            {
                option = new Option(this.openAction.Option.Type, this.openAction.Option.UnderlyingSymbol, currentCandle.Timestamp, (double)currentCandle.Close, this.openAction.Option.StrikePrice, this.openAction.Option.Expiration);
                pos = new TradePosition(currentCandle.Timestamp, this.openAction, option);
            }
            else
            {
                pos = new TradePosition(currentCandle.Timestamp, this.openAction, currentCandle);
            }

             
            if (Strategy.ShouldExit(pos, out var action))
            {
                Transactions.Add((openAction, action, pos));
                Console.WriteLine($"Exited on {currentCandle.Timestamp} at {action.Price:c} with a net change of {pos.GainLoss:c} ({pos.GainLossPercent:P2}%)");
                this.openAction = null;
            }
        }
    }

    public void OnMarketClose(DateTime date)
    {
        throw new NotImplementedException();
    }

    public void OnMarketOpen(DateTime date)
    {
        throw new NotImplementedException();
    }
}
