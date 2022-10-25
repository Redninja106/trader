using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;

internal class MarketRunner
{
    private List<DataSet> dataSets;
    private List<DataSet> staticDataSets;

    public MarketRunner(IEnumerable<DataSet> dataSets, IEnumerable<DataSet>? staticDataSets = null)
    {
        var ds = dataSets.First();
        if(!dataSets.Skip(1).All(d => d.Candles.Count() == ds.Candles.Count() && d.Candles.Select(c=>c.Timestamp).SequenceEqual(ds.Candles.Select(c=>c.Timestamp))))
        {
            throw new ArgumentOutOfRangeException("DataSets must align");
        };

        this.dataSets = new(dataSets);
        this.staticDataSets = staticDataSets?.ToList() ?? new List<DataSet>();
    }

    public void Run(ITrader trader)
    {
        Run(new[] { trader });
    }

    public bool CanTrade => !dataSets.First().IsAtEnd;

    public void Run(IEnumerable<ITrader> traders)
    {
        bool simpleMode = !dataSets.Any(ds => ds.CurrentCandleHasMoreTicks);

        foreach (var trader in traders)
        {
            // AssignMembers(trader.Strategy, dataSets);
            if (simpleMode)
            {
                trader.Pump(dataSets.ToDictionary(d => d.Name, d => d.CurrentCandle as ICandle)); ;
            }
            else
            {
                while (dataSets.Any(ds=>ds.CurrentCandleHasMoreTicks))
                {
                    dataSets.ForEach(ds => ds.Tick());
                    trader.Pump(dataSets.ToDictionary(d => d.Name, d => d.CurrentCandle as ICandle)); ;
                }
            }
        }
        dataSets.ForEach(ds => ds.AdvanceCandle());
    }

    private IEnumerable<DateTime> DataSetDays()
    {
        var startDay = this.dataSets.Min(d => d.Candles.Min(c => c.Timestamp.Date));
        var MaxDay = this.dataSets.Max(d => d.Candles.Max(c => c.Timestamp.Date));

        var currentDay = startDay;

        while(currentDay <= MaxDay)
        {
            if(currentDay.DayOfWeek != DayOfWeek.Saturday && currentDay.DayOfWeek != DayOfWeek.Sunday)
            {
                yield return currentDay;
            }
            
            currentDay = currentDay.AddDays(1);
        }
    }

    //void AssignMembers(IStrategy strategy, IEnumerable<DataSet> dataSets)
    //{
    //    var dataSetViews = dataSets.ToDictionary(d => d.Name, d => d);

    //    var strategyType = strategy.GetType();

    //    var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    //    IEnumerable<FieldInfo> fields = strategyType.GetFields(bindingFlags);

    //    Dictionary<string, string> dataSetLookup = new Dictionary<string, string>();
        
    //    // map data set ids to symbols;
    //    foreach (var field in fields.Where(f=>f.FieldType == typeof(IDataSetView)))
    //    {
    //        var attribute = field.GetCustomAttribute<DataSetAttribute>();
    //        if (attribute != null)
    //        {
    //            var view = field.GetValue(strategy) as IDataSetView;
    //            if (view == null)
    //            {
    //                throw new ArgumentOutOfRangeException($"Data set not found: {view.Name}");
    //            }
    //            dataSetLookup.Add(attribute.Id, view.Name);
    //        }
    //    }


    //    foreach (var field in fields)
    //    {
    //        var useDsAttrib = field.GetCustomAttribute<UsesDataSetAttribute>();
    //        var dsAttrib = field.GetCustomAttribute<DataSetAttribute>();

    //        IDataSetView view = null;
    //        string id = string.Empty;

    //        if(useDsAttrib != null)
    //        {
    //            id = useDsAttrib.Id;
    //        } 
    //        else if(dsAttrib != null)
    //        {
    //            id = dsAttrib.Id;
    //        }
    //        else
    //        {
    //            throw new ArgumentOutOfRangeException("Id not found");
    //        }

    //        if(dataSetViews.ContainsKey(id))
    //        {
    //            view = dataSetViews[id];
    //        }
    //        else
    //        {
    //            string mappedSymbol = dataSetLookup[id];
    //            view = dataSetViews[mappedSymbol];
    //        }

    //        if (field.FieldType == typeof(IDataSetView))
    //        {
    //            field.SetValue(strategy, view);
    //        }
    //        //else if (field.FieldType!.GetInterfaces().Any(i => i == typeof(IAnalyzer)))
    //        //{
    //        //    var analyzer = field.GetValue(strategy) as IAnalyzer;
    //        //    analyzer?.Calculate(view);
    //        //}
    //        //else
    //        //{
    //        //    throw new Exception();
    //        //}
    //    }
    //}

    //Dictionary<string, IEnumerable<T>> GetFields<T>(IStrategy strategy)
    //{
    //    var strategyType = strategy.GetType();

    //    var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    //    IEnumerable<FieldInfo> fields = strategyType.GetFields(bindingFlags);
    //    fields = fields.Where(f => f.DeclaringType!.IsSubclassOf(typeof(T)) || f == typeof(T));
    //    fields = fields.Where(f => f.GetCustomAttribute<DataSetAttribute>() is not null);

    //    return fields.GroupBy(f => f.GetCustomAttribute<DataSetAttribute>()!.Id).ToDictionary(k => k.Key, e => e.Select(f => f.GetValue(strategy)).OfType<T>());
    //}
}
