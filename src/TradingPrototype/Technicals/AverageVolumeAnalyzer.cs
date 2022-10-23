//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace TradingPrototype.Technicals;
//internal class AverageVolumeAnalyzer : Analyzer<double>
//{
//    private int count = 0;

//    public AverageVolumeAnalyzer(int count)
//    {
//        this.count = count;
//    }

//    protected override double InternalCalculate(IDataSetView dataSet)
//    {
//        var candles = dataSet.LookBack(count);
//        return candles.Any() ? candles.Average(c => c.Volume) : 0;
//    }
//}
