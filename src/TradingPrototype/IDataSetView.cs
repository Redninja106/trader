using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype;

internal interface IDataSetView
{
    public string Name { get; }
    Quote Quote { get; }
    ICandle CurrentCandle { get; }
    IEnumerable<ICandle> LookBack(int count);

    T GetTechnicalIndicator<T>(TechnicalIndicator indicator);
    T GetTechnicalIndicator<T>(ICandle candle, TechnicalIndicator indicator);
    T[] GetTechnicals<T>(TechnicalIndicator indicator);
}