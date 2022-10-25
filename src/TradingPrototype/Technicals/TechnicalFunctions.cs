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
        public static float Ema(this ITechnicalAnalysisView data, int periods)
        {
            // var existing = data.GetTechnicals<float>(TechnicalAnalysis.Ema(periods));
            var values = data.Close;
            
            if (values.Length < periods)
            {
                return 0f;
            }

            var result = new decimal[values.Length];

            var ret = TALib.Core.Ma(values, 0, result.Length-1, result, out int outBegIdx, out int outNbElement, Core.MAType.Ema, periods);

            if (ret != TA.RetCode.Success)
            {
                return -1f;
            }

            return (float)result[outNbElement - 1];
        }
        public static float Sma(this ITechnicalAnalysisView data, int periods)
        {
            var values = data.Close;

            if (values.Length < periods)
            {
                return 0;
            }

            var result = new decimal[values.Length];

            var ret = TALib.Core.Ma(values, 0, values.Length - 1, result, out int outBegIdx, out int outNbElement, Core.MAType.Sma, periods);

            if (ret != TA.RetCode.Success)
            {
                return 0;
            }

            return (float)result[outNbElement - 1];
        }

        public static (float K, float D) StochasicFast(this ITechnicalAnalysisView data, int kFastPeriods, int kSlowPeriods, int dSlowPeriods)
        {
            var opens = data.Open;

            if (opens.Length < (new[] { kFastPeriods, kSlowPeriods, dSlowPeriods }).Max())
            {
                return (0f, 0f);
            }

            
            var highs = data.High;
            var lows = data.Low;

            var resultK = new decimal[opens.Length];
            var resultD = new decimal[opens.Length];

            var results = new decimal[opens.Length];

            var ret = TALib.Core.Stoch(highs, lows, opens, 0, results.Length-1, resultK, resultD, out int outBegIdx, out int outNbElement, optInFastKPeriod: kFastPeriods, optInSlowKPeriod: kSlowPeriods, optInSlowDPeriod: dSlowPeriods);

            if (ret != TA.RetCode.Success)
            {
                return new(-1f, -1f);
            }

            return ((float)resultK[outNbElement - 1], (float)resultD[outNbElement - 1]);
        }

        public static long AverageVolume(this ITechnicalAnalysisView data, int periods)
        {
            var candles = data.Volume;
            return (long)(candles.Any() ? candles.Average() : 0);
        }
    }
}
