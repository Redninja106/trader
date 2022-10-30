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
        public static TechnicalResult.Ema Ema(this ITechnicalAnalysisView data, int periods, float expected = 0)
        {
            int lookback = Core.MaLookback(TA.MAType.Ema, periods);

            if (data.Close.Length < periods)
            {
                return new TechnicalResult.Ema(0f);
            }

            //List<decimal> allvalues = new List<decimal>();
            //allvalues.Add(data.Close.Last());
            //var candles = data.LookBack(periods);
            //foreach(var c in candles)
            //{
            //    var t = data.GetTechnicalIndicator<TechnicalResult.Ema>(c, TechnicalAnalysis.Ema(periods));
            //    allvalues.Add((decimal)t.Result);
            //}

            //values[0] = (decimal)t.Result;
            decimal[] values = data.Close;

            //Array.Copy(data.Close, data.Close.Length - periods, values, 0, periods);

            var results = new decimal[values.Length];
            var ret = TALib.Core.Ma(values, 0, values.Length - 1, results, out int outBegIdx, out int outNbElement, Core.MAType.Ema, periods);
            if (ret != TA.RetCode.Success)
            {
                return new TechnicalResult.Ema(-1f);
            }

            return new TechnicalResult.Ema((float)results[outNbElement - 1]);
        }
        public static TechnicalResult.Sma Sma(this ITechnicalAnalysisView data, int periods)
        {
            var values = data.Close;

            if (values.Length < periods)
            {
                return new TechnicalResult.Sma(0);
            }

            var result = new decimal[values.Length];

            var ret = TALib.Core.Ma(values, 0, values.Length - 1, result, out int outBegIdx, out int outNbElement, Core.MAType.Sma, periods);

            if (ret != TA.RetCode.Success)
            {
                return new TechnicalResult.Sma(0);
            }

            return new TechnicalResult.Sma((float)result[outNbElement - 1]);
        }

        public static TechnicalResult.StochasticFast StochasticFast(this ITechnicalAnalysisView data, int kFastPeriods, int kSlowPeriods, int dSlowPeriods)
        {
            var opens = data.Open;

            if (opens.Length < (new[] { kFastPeriods, kSlowPeriods, dSlowPeriods }).Max())
            {
                return new TechnicalResult.StochasticFast(0f, 0f);
            }

            
            var highs = data.High;
            var lows = data.Low;

            var resultK = new decimal[opens.Length];
            var resultD = new decimal[opens.Length];

            var results = new decimal[opens.Length];

            var ret = TALib.Core.Stoch(highs, lows, opens, 0, results.Length-1, resultK, resultD, out int outBegIdx, out int outNbElement, optInFastKPeriod: kFastPeriods, optInSlowKPeriod: kSlowPeriods, optInSlowDPeriod: dSlowPeriods);

            if (ret != TA.RetCode.Success)
            {
                return new TechnicalResult.StochasticFast(-1f, -1f);
            }

            return new TechnicalResult.StochasticFast((float)resultK[outNbElement - 1], (float)resultD[outNbElement - 1]);
        }

        public static long AverageVolume(this ITechnicalAnalysisView data, int periods)
        {
            var candles = data.Volume;
            return (long)(candles.Any() ? candles.Average() : 0);
        }
    }
}
