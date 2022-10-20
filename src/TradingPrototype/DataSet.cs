using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class DataSet
{
    public DataSet(string symbol, IEnumerable<Candle> candles)
    {
        this.Symbol = symbol;
        Candles = new(candles);
    }

    public string Symbol { get; private set; }
    public List<Candle> Candles { get; private set; }

    public static DataSet Load(string symbol, string file)
    {
        return new(symbol, File.ReadAllLines(file).Skip(1).Select(Candle.Parse));
    }
}
