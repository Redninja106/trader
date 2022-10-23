//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TALib;
//using TA = TALib.Core;

//namespace TradingPrototype.Technicals;
//internal class EMAAnalyzer : Analyzer<float>
//{
//    public int Period { get; private set; }

//    public EMAAnalyzer(int period)
//    {
//        Period = period;
//    }

//    protected override float InternalCalculate(IDataSetView dataSet)
//    {
//        var candles = dataSet.LookBack(Int32.MaxValue);
//        var values = candles.Append(dataSet.CurrentCandle)
//            .Select(c => (decimal)c.Close).ToArray();

//        if (values.Length < Period)
//        {
//            return 0;
//        }

//        var result = new decimal[candles.Count()];

//        var ret = TALib.Core.Ma(values, 0, values.Count()-1, result, out int outBegIdx, out int outNbElement, Core.MAType.Ema, Period);

//        if (ret != TA.RetCode.Success)
//        {
//            return 0;
//        }

//        return (float)result[outNbElement - 1];
//    }
//}
