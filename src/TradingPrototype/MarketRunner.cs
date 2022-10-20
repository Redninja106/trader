using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;

internal class MarketRunner
{
    private List<DataSet> dataSets;

    public MarketRunner(IEnumerable<DataSet> dataSets)
    {
        this.dataSets = new(dataSets);
    }

    public void Run(ITrader trader)
    {
        Run(new[] { trader });
    }

    public void Run(IEnumerable<ITrader> traders)
    {
        Dictionary<string, PositionedDataSetView> views = new();

        foreach (var dataSet in dataSets)
        {
            views.Add(dataSet.Symbol, new PositionedDataSetView(dataSet));
        }

        while (!views.First().Value.IsAtEnd)
        {
            var candles = views.ToDictionary(k => k.Key, e => e.Value.MostRecent);

            foreach (var trader in traders)
            {
                AssignMembers(trader.Strategy, views);
                trader.Pump(candles);
            }

            foreach (var view in views)
            {
                view.Value.Advance();
            }
        }
    }

    void AssignMembers(IStrategy strategy, Dictionary<string, PositionedDataSetView> dataSetViews)
    {

        var strategyType = strategy.GetType();

        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        IEnumerable<FieldInfo> fields = strategyType.GetFields(bindingFlags);
        fields = fields.Where(f => f.GetCustomAttribute<DataSetAttribute>() is not null);

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<DataSetAttribute>();
            var view = dataSetViews[attribute!.Symbol];

            if (field.FieldType == typeof(IDataSetView))
            {
                field.SetValue(strategy, view);
            }
            else if (field.FieldType!.IsSubclassOf(typeof(Analyzer)))
            {
                var analyzer = field.GetValue(strategy) as Analyzer;
                analyzer?.Calculate(view);
            }
            else
            {
                throw new Exception();
            }
        }
    }

    Dictionary<string, IEnumerable<T>> GetFields<T>(IStrategy strategy)
    {
        var strategyType = strategy.GetType();

        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        IEnumerable<FieldInfo> fields = strategyType.GetFields(bindingFlags);
        fields = fields.Where(f => f.DeclaringType!.IsSubclassOf(typeof(T)) || f == typeof(T));
        fields = fields.Where(f => f.GetCustomAttribute<DataSetAttribute>() is not null);

        return fields.GroupBy(f => f.GetCustomAttribute<DataSetAttribute>()!.Symbol).ToDictionary(k => k.Key, e => e.Select(f => f.GetValue(strategy)).OfType<T>());
    }
}
