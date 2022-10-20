using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;

internal interface IStrategy
{
    bool ShouldEnter([NotNullWhen(true)] out TradeAction? action);
    bool ShouldExit(TradePosition position, [NotNullWhen(true)] out TradeAction? action);
}