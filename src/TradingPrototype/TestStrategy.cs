using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;
internal class TestStrategy : IStrategy
{
    const string PrimaryDataSet = "Primary";

    IDataSetView? driver;

    public TestStrategy(IDataSetView driver)
    {
        this.driver = driver;
    }

    public bool ShouldEnter([NotNullWhen(true)] out TradeAction? action)
    {
        var emaLow = driver.GetTechnicalIndicator<TechnicalResult.Ema>(TechnicalAnalysis.Ema(8));
        var emaHigh = driver.GetTechnicalIndicator<TechnicalResult.Ema>(TechnicalAnalysis.Ema(21));

        //Console.WriteLine($"Moving avgs: {driver.CurrentCandle.Timestamp} Low: {emaLow}, H: {emaHigh}");
        Console.WriteLine($"{driver!.Name} Candle " + driver.CurrentCandle.ToString()); ;
         
        var divergance = emaLow.Result - emaHigh.Result;
        if (divergance > 0 && divergance > .25d)
        {
            var oc = driver.QueryOptionChain(driver.CurrentCandle.Timestamp.AddMonths(1));
            var o = oc.Puts[oc.Puts.Count/2];

            //action = new(driver.CurrentCandle.Timestamp, TradeAction.Action.Short, new EquitySymbol(driver.Name), 1000, driver.CurrentCandle.Close);
            action = new(driver.CurrentCandle.Timestamp, TradeAction.Action.Long, o, 10, (decimal)o.Price);

            return true;
        }
        else
        {
            action = null;
            return false;
        }
    }

    public bool ShouldExit(TradePosition position, [NotNullWhen(true)] out TradeAction? action)
    {
        var emaLow = driver.GetTechnicalIndicator<TechnicalResult.Ema>(TechnicalAnalysis.Ema(8));
        var emaHigh = driver.GetTechnicalIndicator<TechnicalResult.Ema>(TechnicalAnalysis.Ema(21));

        //Console.WriteLine($"MOVIN Low: {emaLow}, H{emaHigh}");
        //Console.WriteLine($"{driver!.Name} Tick " + driver.Quote.ToString()); ;

        var divergance = emaLow.Result - emaHigh.Result;

        if (position.GainLossPercent > .01m && divergance <= -.10)
        {
            if(position.Option != null)
            {
                action = new(driver.CurrentCandle.Timestamp, TradeAction.Action.Short, position.Option, Math.Abs(position.Quantity), (decimal)position.Option.Price);
            }
            else
            {
                action = new(driver.CurrentCandle.Timestamp, TradeAction.Action.Short, new EquitySymbol(position.Symbol), Math.Abs(position.Quantity), driver!.CurrentCandle.Close);
            }
            return true;
        }
        else
        {
            action = null;
            return false;
        }
    }
}