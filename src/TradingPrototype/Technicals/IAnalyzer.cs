//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TradingPrototype.Technicals;

//internal abstract class Analyzer<T> : IAnalyzer<T>
//{
//    private Dictionary<ICandle, T> history = new Dictionary<ICandle, T>();

//    public Analyzer()
//    {
//    }

//    public T Current 
//    {
//        get; private set;
//    }

//    public void Calculate(IDataSetView dataset)
//    {
//        var value = this.InternalCalculate(dataset);

//        if (history.ContainsKey(dataset.CurrentCandle))
//        {
//            history.Remove(dataset.CurrentCandle);
//        }

//        history.Add(dataset.CurrentCandle, value);

//        this.Current = value;
//    }
//    protected abstract T InternalCalculate(IDataSetView dataSet);

//    public virtual IEnumerable<T> LookBack(int count)
//    {
//        return history.Reverse().Skip(1).Take(count).Select(h => h.Value).ToArray();
//    }
//}

//internal interface IAnalyzer<T>
//{
//    T Current { get; }
//    void Calculate(IDataSetView dataSet);
//}

