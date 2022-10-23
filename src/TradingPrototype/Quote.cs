using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype
{
    internal class Quote
    {
        public Quote(decimal bid, decimal ask, decimal last)
        {
            this.Bid = bid;
            this.Ask = ask;
            this.Last = last;
        }

        public decimal Bid { get; private set; }
        public decimal Ask { get; private set; }
        public decimal Last { get; private set; }

        public override string ToString()
        {
            return $"B: {this.Bid:c}, A:{this.Ask:c}, L:{this.Last:c}";
        }
    }
}
