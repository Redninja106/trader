using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype.Options
{
    internal class OptionChain
    {
        public OptionChain(DateTime asOf, EquitySymbol equitySymbol, double currentPrice, DateTime expiration, int range = 50)
        {
            List<Option> puts = new List<Option>();
            List<Option> calls = new List<Option>();

            for (int i = -range; i < range; i++)
            {
                double strike = Math.Truncate(currentPrice) + i;

                puts.Add(new Option(OptionContractType.Put, equitySymbol, asOf, currentPrice, strike, expiration));
                calls.Add(new Option(OptionContractType.Call, equitySymbol, asOf, currentPrice, strike, expiration));
            }

            this.Puts = puts;
            this.Calls = calls;
        }

        public IList<Option> Puts { get; private set; }
        public IList<Option> Calls { get; private set; }

    }
}
