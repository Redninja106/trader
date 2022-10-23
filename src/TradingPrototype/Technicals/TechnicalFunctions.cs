using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TALib;
using TA = TALib.Core;

namespace TradingPrototype.Technicals
{
    internal static class TechnicalFunctions
    {
        public static float Ema(this IDataSetView data, int periods)
        {
            var candles = data.LookBack(Int32.MaxValue);
            var values = candles.Append(data.CurrentCandle)
                .Select(c => (decimal)c.Close).ToArray();

            if (values.Length < periods)
            {
                return 0;
            }

            var result = new decimal[candles.Count()];

            var ret = TALib.Core.Ma(values, 0, values.Count()-1, result, out int outBegIdx, out int outNbElement, Core.MAType.Ema, periods);

            if (ret != TA.RetCode.Success)
            {
                return 0;
            }

            return (float)result[outNbElement - 1];
        }
        public static float Sma(this IDataSetView data, int periods)
        {
            var candles = data.LookBack(Int32.MaxValue);
            var values = candles.Append(data.CurrentCandle)
                .Select(c => (decimal)c.Close).ToArray();

            if (values.Length < periods)
            {
                return 0;
            }

            var result = new decimal[candles.Count()];

            var ret = TALib.Core.Ma(values, 0, values.Count()-1, result, out int outBegIdx, out int outNbElement, Core.MAType.Sma, periods);

            if (ret != TA.RetCode.Success)
            {
                return 0;
            }

            return (float)result[outNbElement - 1];
        }

        public static long AverageVolume(this IDataSetView data, int periods)
        {
            var candles = data.LookBack(periods);
            return (long)(candles.Any() ? candles.Average(c => c.Volume) : 0);
        }
    }
}
