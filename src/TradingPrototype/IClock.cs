﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype
{
    internal interface IClock
    {
        void Set(DateTime dt);
        DateTime Now { get; }
    }
}
