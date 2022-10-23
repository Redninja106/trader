//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TA = TALib.Core;

//namespace TradingPrototype.Technicals;
//internal class SMAAnalyzer : Analyzer<decimal>
//{
//    private IDataSetView data;

//    public int Period { get; private set; }

//    public SMAAnalyzer(int period)
//    {
//        Period = period;
//    }

//    public override void Calculate(IDataSetView data)
//    {
//        var values = data.LookBack(Period).Append(data.CurrentCandle).Select(c => (double)c.Close).ToArray();

//        if (values.Length <= 1)
//        {
//            SetValue(data.CurrentCandle, data.CurrentCandle.Close);
//            return;
//        }

//        var result = new double[values.Length];
//        var ret = TA.Sma(values, 0, values.Length - 1, result, out int begin, out int count, Period);
        
//        if (ret != TA.RetCode.Success)
//        {
//            SetValue(data.CurrentCandle, 0);
//            return;
//        }

//        SetValue(data.CurrentCandle, (decimal)result.FirstOrDefault());
//    }

//    public override IEnumerable<decimal> LookBack(int count)
//    {
//        return null;
//    }
//}
