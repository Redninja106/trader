using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype.Technicals
{
    internal class TechnicalIndicator
    {
        public Func<IDataSetView, object> Calculate;
        public TechnicalIndicator(string signature, Func<IDataSetView, object> calculation)
        {
            this.Signature = signature;
            this.Calculate = calculation;
        }
        public string Signature { get; protected set; }
    }


    internal static class TechnicalAnalysis
    {
        private static List<TechnicalIndicator> indicators = new List<TechnicalIndicator>();

        public static TechnicalIndicator Ema(int periods)
        {
            return AddOrGet($"ema{periods}", ds => ds.Ema(periods));
        }

        public static TechnicalIndicator Sma(int periods)
        {
            return AddOrGet($"sma{periods}", ds => ds.Sma(periods));
        }

        public static TechnicalIndicator AverageVolume(int periods)
        {
            return AddOrGet($"AvgVolume{periods}", ds => ds.AverageVolume(periods));
        }

        private static TechnicalIndicator AddOrGet(string signature, Func<IDataSetView, object> calculation)
        {
            var indicator = indicators.SingleOrDefault(i => i.Signature == signature);

            if(indicator == null)
            {
                indicator = new TechnicalIndicator(signature, calculation);
                indicators.Add(indicator);
            }

            return indicator;
        }
    }

}
