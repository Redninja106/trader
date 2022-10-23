using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal class UsesDataSetAttribute : Attribute
{
    public string Id { get; }

    public UsesDataSetAttribute(string id)
    {
        this.Id = id;
    }
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal class DataSetAttribute : Attribute
{
    public string Id { get; }

    public DataSetAttribute(string id)
    {
        this.Id = id;
    }
}
