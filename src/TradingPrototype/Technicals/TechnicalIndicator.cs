using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype.Technicals
{
    internal class TechnicalIndicator
    {
        public Func<ITechnicalAnalysisView, object> Calculate;
        public TechnicalIndicator(string signature, Func<ITechnicalAnalysisView, object> calculation)
        {
            this.Signature = signature;
            this.Calculate = calculation;
        }
        public string Signature { get; protected set; }
    }
}
