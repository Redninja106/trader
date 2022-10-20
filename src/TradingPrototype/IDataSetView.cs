using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal interface IDataSetView
{
    Candle MostRecent { get; }
    IEnumerable<Candle> LookBack(int count);
}
