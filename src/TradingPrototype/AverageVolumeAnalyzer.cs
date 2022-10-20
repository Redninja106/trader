using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class AverageVolumeAnalyzer : Analyzer
{
    private int count = 0;

    public double AverageVolume { get; private set; }

    public AverageVolumeAnalyzer(int count)
    {
        this.count = count;
    }

    public override void Calculate(IDataSetView data)
    {
        var candles = data.LookBack(this.count);
        AverageVolume = candles.Any() ? candles.Average(c => c.Volume) : 0;
    }
}
