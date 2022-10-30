using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Technicals;

namespace TradingPrototype
{
    internal interface IChartView
    {
        ICandle Candles { get; }
        IDictionary<ICandle, IEnumerable<TechnicalIndicator>> Technicals { get; }
    }
}
