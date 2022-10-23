namespace TradingPrototype;

public class Tick
{
    public Tick(decimal price, long volume)
    {
        Price = price;
        Volume = volume;
    }

    public decimal Price { get; set; }
    public long Volume { get; set; }
}
