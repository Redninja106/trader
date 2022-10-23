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
        var emaLow = driver.GetTechnicalIndicator<float>(TechnicalAnalysis.Ema(8));
        var emaHigh = driver.GetTechnicalIndicator<float>(TechnicalAnalysis.Ema(21));

        //Console.WriteLine($"Moving avgs: {driver.CurrentCandle.Timestamp} Low: {emaLow}, H: {emaHigh}");
        //Console.WriteLine($"{driver!.Name} Candle " + driver.CurrentCandle.ToString()); ;
         
        var divergance = Math.Abs(emaHigh - emaLow);
        if (emaHigh > emaLow && divergance >.20)
        {
            action = new(TradeAction.Action.Short, driver.Name, 1000, driver.CurrentCandle);
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
        var emaLow = driver.GetTechnicalIndicator<float>(TechnicalAnalysis.Ema(8));
        var emaHigh = driver.GetTechnicalIndicator<float>(TechnicalAnalysis.Ema(21));

        //Console.WriteLine($"MOVIN Low: {emaLow}, H{emaHigh}");
        //Console.WriteLine($"{driver!.Name} Tick " + driver.Quote.ToString()); ;

        var divergance = Math.Abs(emaHigh - emaLow);

        if (position.GainLossPercent > .01m || emaLow > emaHigh && divergance >= .25)
        {
            action = new(TradeAction.Action.Long, position.Symbol, Math.Abs(position.Quantity), driver!.CurrentCandle);
            return true;
        }
        else
        {
            action = null;
            return false;
        }
    }
}