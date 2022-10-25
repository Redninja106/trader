namespace TradingPrototype.Technicals
{
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

        private static TechnicalIndicator AddOrGet(string signature, Func<ITechnicalAnalysisView, object> calculation)
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
