using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;
//internal interface IDataSetView
//{
//    public string Symbol { get; }
//    Quote Tick { get; }
//    ICandle MostRecent { get; }
//    IEnumerable<ICandle> LookBack(int count);
//}

internal interface IDataSetView
{
    public string Name { get; }
    Quote Quote { get; }
    ICandle CurrentCandle { get; }
    IEnumerable<ICandle> LookBack(int count);

    T GetTechnicalIndicator<T>(TechnicalIndicator indicator);
    T GetTechnicalIndicator<T>(ICandle candle, TechnicalIndicator indicator);
}

//internal class DyanmicDataSetView : IDataSetView
//{
//    private string symbol;

//    public static IDataSetView FromSymbol(string symbol)
//    {
//        var v = new DyanmicDataSetView();
//        v.symbol = symbol;
//        return v;
//    }
//    string IDataSetView.Name { get { return symbol; } }
//    Quote IDataSetView.Quote { get; }
//    ICandle IDataSetView.CurrentCandle { get; }

//    IEnumerable<ICandle> IDataSetView.LookBack(int count)
//    {
//        throw new NotImplementedException();
//    }
//}