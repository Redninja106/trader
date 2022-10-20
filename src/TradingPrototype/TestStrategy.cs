using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class TestStrategy : IStrategy
{
    [DataSet(Symbols.SPY)]
    AverageVolumeAnalyzer volumeAnalyzer;

    [DataSet(Symbols.SPY)]
    IDataSetView spy;

    [DataSet(Symbols.VIX)]
    IDataSetView Vix;

    [DataSet(Symbols.SPY)]
    EMAAnalyzer ema = new(10);

    public TestStrategy(AverageVolumeAnalyzer volumeAnalyzer)
    {
        this.volumeAnalyzer = volumeAnalyzer;
    }

    public bool ShouldEnter([NotNullWhen(true)] out TradeAction? action)
    {
        Console.WriteLine("MOVIN AVG " + ema.MovingAverage);

        if (volumeAnalyzer.AverageVolume > 60_000_000)
        {
            action = new(Symbols.SPY, 10, spy.MostRecent);
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
        Console.WriteLine("MOVIN AVG " + ema.MovingAverage);

        if (position.GainLoss >= 1f)
        {
            action = new(position.Symbol, position.Quantity, spy.MostRecent);
            return true;
        }
        else
        {
            action = null;
            return false;
        }
    }
}