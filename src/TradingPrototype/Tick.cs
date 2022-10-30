namespace TradingPrototype;

internal class Tick
{
    public Tick(decimal price, long volume)
    {
        Price = price;
        Volume = volume;
    }

    public decimal Price { get; set; }
    public long Volume { get; set; }

    public static Tick[] CreateTicks(ICandle candle, int tickCount)
    {
        if (tickCount <= 4)
        {
            throw new ArgumentException("tickCount must be greater than 4", nameof(tickCount));
        }

        // calculate prices
        decimal[] ticks = new decimal[tickCount];
        for (int i = 0; i < tickCount; i++)
        {
            decimal progress = i / (tickCount - 1m);
            progress *= 3;

            decimal x = progress % 1.0m;
            var (m, b) = progress switch
            {
                <= 0 => (0m, candle.Open),
                <= 1 => (candle.High - candle.Open, candle.Open),
                <= 2 => (candle.Low - candle.High, candle.High),
                <= 3 => (candle.Close - candle.Low, candle.Low),
                _ => (0m, candle.Close)
            };

            decimal price = m * x + b;

            ticks[i] = price;
        }

        // calculate volume
        long[] volume = new long[tickCount];
        for (int i = 0; i < tickCount; i++)
        {
            volume[i] = candle.Volume / tickCount;
        }

        return ticks.Zip(volume).Select(t => new Tick(t.First, t.Second)).ToArray();
    }
}