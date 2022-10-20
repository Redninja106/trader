using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal class DataSetAttribute : Attribute
{
    public string Symbol { get; }

    public DataSetAttribute(string symbol)
    {
        this.Symbol = symbol;
    }
}
