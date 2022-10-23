namespace TradingPrototype
{
    internal interface ICandle
    {
        decimal Close { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Open { get; }
        DateTime Timestamp { get; }
        long Volume { get; }
    }
}