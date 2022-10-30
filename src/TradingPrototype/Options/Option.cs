using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype.Options
{
    [System.Diagnostics.DebuggerDisplay("{Symbol}{Price}")]
    internal class Option : IOption
    {
        public static TimeSpan DefaultExpirationTime => new TimeSpan(16, 0, 0);

        private BlackScholesPricer prices;
        private EquitySymbol symbol;

        public Option(OptionContractType type, EquitySymbol symbol, DateTime asOf, double currentPrice, double strike, DateTimeOffset expiry)
        {
            this.symbol = symbol;
            this.Type = type;
            this.Expiration = expiry;
            this.StrikePrice = strike;
            this.prices = BlackScholesPricer.Calculate(type, currentPrice, (double)strike, BlackScholesPricer.BlackScholes.T(expiry, asOf), symbol.IV, symbol.RiskFreeRate, 0);
            //365, 364, 1d/365d, .31, .01, 0
        }

        public EquitySymbol UnderlyingSymbol => this.symbol;

        public string Symbol
        {
            get
            {
                return $"-{this.UnderlyingSymbol}{this.Expiration:yyyyMMdd}{(Type == OptionContractType.Call ? "C" : "P")}{this.StrikePrice:00}";
            }
        }

        public DateTimeOffset Expiration { get; private set; }

        public double StrikePrice { get; private set; }

        public OptionContractType Type { get; private set; }

        public double Price => prices.Price;
        public double Delta => prices.Delta;
        public double Gamma => prices.Gamma;
        public double Theta => prices.Theta;
        public double Vega => prices.Vega;
        public double Rho => prices.Vega;
    }
}
