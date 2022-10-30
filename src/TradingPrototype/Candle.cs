using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;

internal class Candle : ICandle
{
    public Candle(DateTime timestamp, decimal open, decimal high, decimal low, decimal close, long volume)
    {
        Timestamp = timestamp;

        this.Open = open;
        this.High = high;
        this.Low = low;
        this.Close = close;
        this.Volume = volume;
    }

    public static Candle FromTicks(DateTime timestamp, IEnumerable<Tick> ticks)
    {
        Candle c = Candle.Zero(timestamp);
        c.Open = ticks.First().Price;
        c.Close = ticks.Last().Price;
        c.Low = ticks.Min(t => t.Price);
        c.High = ticks.Max(t => t.Price);
        c.Volume = ticks.Sum(t => t.Volume);
        return c;
    }
    
    public static Candle Zero(DateTime timeStamp)
    {
        Candle c = new Candle(timeStamp, 0m, 0m, 0m, 0m, 0);

        return c;
    }

    public void Tick(Tick tick)
    {
        if (this.Open == 0) // new candle
            this.Open = tick.Price;

        if (tick.Price > this.High || tick.Price == 0 || this.High == 0)
            this.High = tick.Price;

        if (tick.Price < this.Low || tick.Price == 0 || this.Low == 0)
            this.Low = tick.Price;

        this.Close = tick.Price;
        this.Volume += tick.Volume;
    }

    public void Tick(IEnumerable<Tick> ticks)
    {
        foreach(var t in ticks)
        {
            this.Tick(t);
        }
    }

    public DateTime Timestamp { get; private set; }
    public decimal Open { get; private set; }
    public decimal High { get; private set; }
    public decimal Low { get; private set; }
    public decimal Close { get; private set; }
    public long Volume { get; private set; }

    public override string? ToString()
    {
        return $"{{Time: {Timestamp}, O: {Open:c}, H: {High:c}, L: {Low:c}, C: {Close:c}, V: {Volume} }}";
    }

    public static Candle Parse(string data)
    {
        var parts = data.Split(',');
        
        // Yahoo
        // Candle c = new Candle(DateTime.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4]), long.Parse(parts[6]));
        
        // fidelity
        Candle c = new Candle(DateTime.Parse(parts[0] + " " + parts[1]), decimal.Parse(parts[2]), decimal.Parse(parts[3]), decimal.Parse(parts[4]), decimal.Parse(parts[5]), long.Parse(parts[6]));

        return c;
    }
}