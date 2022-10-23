using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;

internal class Candle : ICandle
{
    private List<Tick> ticks = new();
    private int currentPosition = 0;

    public event EventHandler<EventArgs> OnTick;

    public Candle(DateTime timestamp, decimal open, decimal high, decimal low, decimal close, long volume)
    {
        Timestamp = timestamp;

        ticks.Add(new Tick() { Price = open, Volume = 1 });
        ticks.Add(new Tick() { Price = high, Volume = 1 });
        ticks.Add(new Tick() { Price = low, Volume = 1 });
        ticks.Add(new Tick() { Price = close, Volume = 1 });

        this.Reset();

        this.Volume = volume;
    }

    public Candle(DateTime timestamp, IEnumerable<Tick> ticks)
    {
        this.Timestamp = timestamp;

        this.ticks.AddRange(ticks);
        this.Reset();
    }

    public static Candle FromTicks(DateTime timestamp, IEnumerable<Tick> ticks)
    {
        Candle c = new Candle(timestamp, ticks);

        c.Reset();

        while (c.MoreTicks)
        {
            c.Tick();
        }

        return c;
    }
    
    public void Reset()
    {
        currentPosition = -1;
        this.Open = 0m;
        this.High = 0m;
        this.Low = 0m;
        this.Close = 0m;
        this.Volume = 0;
    }

    public bool MoreTicks => currentPosition + 1 < ticks.Count;

    public void TickAll()
    {
        while (this.MoreTicks)
        {
            this.Tick();
        }
    }
    public void Tick()
    {
        if (!MoreTicks)
            return;

        currentPosition++;

        var t = ticks[currentPosition];

        if (currentPosition == 0)
            this.Open = t.Price;

        if (t.Price > this.High)
            this.High = t.Price;

        if (t.Price < this.Low || this.Low == 0)
            this.Low = t.Price;

        this.Close = t.Price;

        if (this.OnTick != null)
        {
            this.OnTick(this, EventArgs.Empty);
        }
    }

    public Tick CurrentTick => ticks[currentPosition];

    public DateTime Timestamp { get; private set; }
    public decimal Open { get; private set; }
    public decimal High { get; private set; }
    public decimal Low { get; private set; }
    public decimal Close { get; private set; }
    public long Volume { get; private set; }

    public override string? ToString()
    {
        return $"{{Time: {Timestamp}, O: {Open}, H: {High}, L: {Low}, C: {Close}, V: {Volume} }}";
    }

    public static Candle Parse(string data)
    {
        var parts = data.Split(',');
        //       Candle c = new Candle(DateTime.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4]), long.Parse(parts[6]));
        Candle c = new Candle(DateTime.Parse(parts[0] + " " + parts[1]), decimal.Parse(parts[2]), decimal.Parse(parts[3]), decimal.Parse(parts[4]), decimal.Parse(parts[5]), long.Parse(parts[6]));

        return c;
    }

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
            decimal price = 0;
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

            decimal y = m * x + b;

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