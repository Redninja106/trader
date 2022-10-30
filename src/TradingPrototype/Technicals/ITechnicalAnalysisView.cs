namespace TradingPrototype.Technicals;

internal interface ITechnicalAnalysisView
{
    decimal[] Open { get; }
    decimal[] Close { get; }
    decimal[] High { get; }
    decimal[] Low { get; }
    long[] Volume { get; }

    DateTime[] TimeStamp { get; }

    IEnumerable<ICandle> LookBack(int count);

    T[] GetTechnicals<T>(TechnicalIndicator indicator) where T : TechnicalResult;
    T GetTechnicalIndicator<T>(ICandle candle, TechnicalIndicator indicator) where T : TechnicalResult;

}