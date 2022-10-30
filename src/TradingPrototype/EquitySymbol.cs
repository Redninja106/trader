using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype
{
    internal class EquitySymbol
    {
        public static double DefaultRiskFreeRate => .01;
        public static double DefaultIV => .18;

        public EquitySymbol(string symbol)
            : this(symbol, DefaultRiskFreeRate, DefaultIV)
        {
        }

        public EquitySymbol(string symbol, double riskFreeRate, double iV)
        {
            Symbol = symbol;
            RiskFreeRate = riskFreeRate;
            IV = iV;
        }

        public string Symbol { get; private set; }

        public double RiskFreeRate { get; private set; }

        public double IV { get; private set; }
    }
}
