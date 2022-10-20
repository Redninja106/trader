using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;

internal class Candle
{
    public Candle(DateTime timestamp, float open, float high, float low, float close, long volume)
    {
        Timestamp = timestamp;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }

    public DateTime Timestamp { get; private set; }
    public float Open { get; private set; }
    public float High { get; private set; }
    public float Low { get; private set; }
    public float Close { get; private set; }
    public long Volume { get; private set; }

    public override string? ToString()
    {
        return $"{{Time: {Timestamp}, Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume} }}";
    }

    public static Candle Parse(string data)
    {
        var parts = data.Split(',');
        Candle c = new Candle(DateTime.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4]), long.Parse(parts[6]));
        return c;
    }
}
