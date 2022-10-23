using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
//internal class PositionedDataSetView : IDataSetView
//{
//    private Candle[] candles;
//    private int currentIndex;

//    public PositionedDataSetView(DataSet dataSet)
//    {
//        this.Symbol = dataSet.Symbol;
//        this.candles = dataSet.Candles.ToArray();
//    }
//    public PositionedDataSetView(string symbol, IEnumerable<Candle> candles)
//    {
//        this.Symbol = symbol;
//        this.candles = candles.ToArray();
//    }

//    public static IDataSetView FromSymbol(string symbol)
//    {
//        var dataSet = new DataSet(symbol, Array.Empty<Candle>());
//        return new PositionedDataSetView(dataSet);
//    }

//    public bool IsAtEnd => currentIndex >= candles.Length;

//    public ICandle MostRecent => this.candles[currentIndex];

//    public Candle CurrentCandle => this.candles[currentIndex];

//    public Quote Tick => new Quote(MostRecent.Close - .01m, MostRecent.Close, MostRecent.Close);

//    public string Symbol { get; private set; }

//    public IEnumerable<ICandle> LookBack(int count)
//    {
//        for (int i = Math.Max(0, currentIndex - count); i < currentIndex; i++)
//        {
//            yield return this.candles[i];
//        }
//    }

//    public void Advance()
//    {
//        currentIndex++;
//    }
//}

interface ITick
{
    public ICandle Candle { get; }
    public Quote Quote { get; }
    public IEnumerable<ITick> Lookback { get; }
}