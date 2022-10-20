﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal interface ITrader
{
    IStrategy Strategy { get; }
    public void Pump(Dictionary<string, Candle> candles);
}
