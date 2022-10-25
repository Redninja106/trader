using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;
internal class DataSet : IDataSetView, ITechnicalAnalysisView
{
    private class CandleData
    {
        public CandleData()
        {
            this.Technicals = new Dictionary<TechnicalIndicator, object>();
            this.Ticks = new List<Tick>();
        }
        public int CurrentTickIndex = 0;

        public bool HasMoreTicks => this.Ticks.Count > 0 && CurrentTickIndex + 1 < Ticks.Count();

        public Dictionary<TechnicalIndicator, object> Technicals { get; private set; }
        public List<Tick> Ticks { get; private set; }
    }

    private int currentCandleIndex = 0;

    private KeyValuePair<Candle, CandleData> currentCandleData;
    
    private Dictionary<Candle, CandleData> candleData = new Dictionary<Candle, CandleData>();

    /// <summary>
    /// Flatten view of techincal data
    /// </summary>
    DateTime[] date;
    decimal[] open;
    decimal[] close;
    decimal[] high;
    decimal[] low;
    long[] volume;

    public DataSet(string name, IEnumerable<Candle> candles)
        : this(name, name, candles)
    {
    }

    public DataSet(string name, string symbol, IEnumerable<Candle> candles)
    {
        this.Name = name;
        this.Symbol = symbol;

        int bufferLength = candles.Count();
        date = new DateTime[bufferLength];
        volume = new long[bufferLength];
        open = new decimal[bufferLength];
        high = new decimal[bufferLength];
        low = new decimal[bufferLength];
        close = new decimal[bufferLength];

        foreach (var c in candles)
        {
            candleData.Add(c, new CandleData());
        }

        currentCandleData = candleData.FirstOrDefault();

        this.UpdateTechnicals();
    }

    public bool CurrentCandleHasMoreTicks => this.currentCandleData.Value.HasMoreTicks;

    public void EmulateRealTime(int resolution = 60)
    {
        foreach(var c in candleData)
        {
            c.Value.Ticks.AddRange(Candle.CreateTicks(c.Key, resolution));
        }
    }

    private void UpdateTechnicals()
    {
        var c = currentCandleData.Key;

        date[this.currentCandleIndex] = c.Timestamp;
        open[this.currentCandleIndex] = c.Open;
        close[this.currentCandleIndex] = c.Close;
        high[this.currentCandleIndex] = c.High;
        low[this.currentCandleIndex] = c.Low;
        volume[this.currentCandleIndex] = c.Volume;

        var data = currentCandleData.Value;

        var techs = currentCandleData.Value.Technicals.Keys.ToArray();
        currentCandleData.Value.Technicals.Clear();
        Array.ForEach(techs, t =>
        {
            var result = t.Calculate(this);
            currentCandleData.Value.Technicals.Add(t, result);
        });
    }

    public void AddTechnical(TechnicalIndicator indicator)
    {
        // add a place holder for the indicator to each candle
        foreach (var c in candleData)
        {
            c.Value.Technicals.Add(indicator, indicator.Calculate(this));
        }
    }   

    public string Name { get; private set; }
    public string Symbol { get; private set; }
    public IEnumerable<Candle> Candles => candleData.Keys;

    public static DataSet Load(string symbol, string file)
    {
        return new(symbol, File.ReadAllLines(file).Skip(1).Select(Candle.Parse));
    }

    public bool IsAtEnd => currentCandleIndex >= candleData.Count;

    public int CurrentIndex => this.currentCandleIndex;

    public ICandle CurrentCandle => this.currentCandleData.Key;

    public Quote Quote => new Quote(this.CurrentCandle.Close - .01m, this.CurrentCandle.Close, this.CurrentCandle.Close);

    decimal[] ITechnicalAnalysisView.Open => this.open.Take(this.currentCandleIndex + 1).ToArray();
    decimal[] ITechnicalAnalysisView.Close => this.close.Take(this.currentCandleIndex + 1).ToArray();
    decimal[] ITechnicalAnalysisView.High => this.high.Take(this.currentCandleIndex + 1).ToArray();
    decimal[] ITechnicalAnalysisView.Low => this.low.Take(this.currentCandleIndex + 1).ToArray();
    long[] ITechnicalAnalysisView.Volume => this.volume.Take(this.currentCandleIndex + 1).ToArray();
    DateTime[] ITechnicalAnalysisView.TimeStamp => this.date.Take(this.currentCandleIndex + 1).ToArray();

    public IEnumerable<ICandle> LookBack(int count)
    {
        for (int i = Math.Max(0, currentCandleIndex - count); i < currentCandleIndex; i++)
        {
            yield return this.candleData.ElementAt(i).Key;
        }
    }

    // ticks the current Candle if in realtime mode
    public bool Tick()
    {
        var candleData = this.currentCandleData;

        if (candleData.Value.Ticks.Count == 0)
        {
            throw new Exception("No Ticks in candle");
        }

        if(candleData.Value.HasMoreTicks)
        {
            var tick = candleData.Value.Ticks[candleData.Value.CurrentTickIndex];
            candleData.Key.Tick(tick);

            // Update Technicals
            this.UpdateTechnicals();

            candleData.Value.CurrentTickIndex++;
        }

        return candleData.Value.HasMoreTicks;
    }

    /// <summary>
    /// Advances to the next candle
    /// </summary>
    public void AdvanceCandle()
    {
        if (this.CurrentCandleHasMoreTicks)
            throw new ArgumentOutOfRangeException("Current candle is not done ticking");

        currentCandleIndex++;

        if(!IsAtEnd)
        {
            this.currentCandleData = candleData.ElementAt(currentCandleIndex);

            // non-realtime is whole bars, so advance and recalculate as tick should not work
            if (this.currentCandleData.Value.Ticks.Count == 0)
            {
                this.UpdateTechnicals();
            }
        }
    }

    public T GetTechnicalIndicator<T>(TechnicalIndicator indicator)
    {
        var ta = this.currentCandleData.Value.Technicals[indicator];

        return (T)ta;
    }
    public T GetTechnicalIndicator<T>(ICandle candle, TechnicalIndicator indicator)
    {
        var ta = this.candleData[(Candle)candle].Technicals[indicator];

        return (T)ta;
    }

    public T[] GetTechnicals<T>(TechnicalIndicator indicator)
    {
        var candleData = this.candleData.Take(this.currentCandleIndex).Select(t => t.Value.Technicals[indicator]);
        return candleData.Select(t => (T)t).ToArray();
    }
}
