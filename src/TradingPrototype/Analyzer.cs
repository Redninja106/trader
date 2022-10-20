using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;

internal abstract class Analyzer
{
    public abstract void Calculate(IDataSetView data);
}
