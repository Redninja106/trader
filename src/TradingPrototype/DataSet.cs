using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;
internal class DataSet : IDataSetView
{
    private int currentIndex = 0;

    private List<TechnicalIndicator> technicals = new List<TechnicalIndicator>();

    private Dictionary<Candle, List<(TechnicalIndicator Indicator, object Value)>> analysis = new Dictionary<Candle, List<(TechnicalIndicator Indicator, object Value)>>();

    public DataSet(string name, IEnumerable<Candle> candles)
        : this(name, name, candles)
    {
    }

    public DataSet(string name, string symbol, IEnumerable<Candle> candles)
    {
        this.technicals = new List<TechnicalIndicator>();
        this.Name = name;
        this.Symbol = symbol;
        Candles = new(candles);
        Candles.ForEach(c => c.OnTick+=this.OnCandleTick_OnTick);
    }

    private void OnCandleTick_OnTick(object? sender, EventArgs e)
    {
        var c = (Candle)sender;
        var data = new List<(TechnicalIndicator Indicator, object Value)>();

        if (!analysis.ContainsKey(c))
        {
            analysis.Add(c, data);
        }
        else
        {
            data = analysis[c];
        }
        data.Clear();

        foreach(var t in technicals)
        {
            data.Add(new (t, t.Calculate(this)));
        }
    }

    public void AddTechnical(TechnicalIndicator indicator)
    {
        this.technicals.Add(indicator);
    }   

    public string Name { get; private set; }
    public string Symbol { get; private set; }
    public List<Candle> Candles { get; private set; }

    public static DataSet Load(string symbol, string file)
    {
        return new(symbol, File.ReadAllLines(file).Skip(1).Select(Candle.Parse));
    }

    public bool IsAtEnd => currentIndex >= Candles.Count;

    public int CurrentIndex => this.currentIndex;

    public ICandle CurrentCandle => this.Candles[currentIndex];

    public Quote Quote => new Quote(this.CurrentCandle.Close - .01m, this.CurrentCandle.Close, this.CurrentCandle.Close);

    public IEnumerable<ICandle> LookBack(int count)
    {
        for (int i = Math.Max(0, currentIndex - count); i < currentIndex; i++)
        {
            //var candle = this.Candles[i];

            // Need to allow previous day candle
            //
            //                || candle.Timestamp.Date < this.CurrentCandle.Timestamp.Date
            //        && candle.Timestamp.Hour == 16 && candle.Timestamp.Minute == 0)
            //if (candle.Timestamp.Date == this.CurrentCandle.Timestamp.Date)
            //{
                yield return this.Candles[i];
            //}
        }
    }

    public void Advance()
    {
        currentIndex++;
    }

    public T GetTechnicalIndicator<T>(TechnicalIndicator indicator)
    {
        return (T)this.analysis[this.Candles[this.currentIndex]].SingleOrDefault(a=>a.Indicator == indicator).Value;
    }
    public T GetTechnicalIndicator<T>(ICandle candle, TechnicalIndicator indicator)
    {
        int index = this.Candles.IndexOf((Candle)candle);

        return (T)this.analysis[this.Candles[index]].SingleOrDefault(t => t.Indicator ==indicator).Value;
    }

}
