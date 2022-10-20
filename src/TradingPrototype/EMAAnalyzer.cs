using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA = TALib.Core;

namespace TradingPrototype;
internal class EMAAnalyzer : Analyzer
{
    public int Period { get; private set; }
    public float MovingAverage { get; private set; }

    public EMAAnalyzer(int period)
    {
        this.Period = period;
    }

    public override void Calculate(IDataSetView data)
    {
        var values = data.LookBack(Period-1).Append(data.MostRecent).Select(c => (double)c.Close).ToArray();

        if (values.Length <= 1)
        {
            MovingAverage = data.MostRecent.Close;
            return;
        }

        var result = new double[values.Length];
        var ret = TA.Ema(values, 0, values.Length-1, result, out int begin, out int count, Math.Min(values.Length, Period));

        if (ret != TA.RetCode.Success)
        {
            MovingAverage = 0;
            return;
        }

        this.MovingAverage = (float)result.FirstOrDefault();
    }
}
