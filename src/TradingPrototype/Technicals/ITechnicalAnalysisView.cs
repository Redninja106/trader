namespace TradingPrototype.Technicals;

internal interface ITechnicalAnalysisView
{
    decimal[] Open { get; }
    decimal[] Close { get; }
    decimal[] High { get; }
    decimal[] Low { get; }
    long[] Volume { get; }

    DateTime[] TimeStamp { get; }

    T[] GetTechnicals<T>(TechnicalIndicator indicator);
}