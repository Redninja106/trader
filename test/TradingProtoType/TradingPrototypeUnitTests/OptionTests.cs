

using System.Security.Cryptography.X509Certificates;
using TradingPrototype;
using TradingPrototype.Options;
using TradingPrototype.Technicals;

namespace TradingPrototypeUnitTests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void OptionTest01()
        {
            var o = new Option(OptionContractType.Put, new EquitySymbol("SPY"), DateTime.Now, 365, 364, DateTime.Now.AddDays(2).Date.Add(Option.DefaultExpirationTime));


            var c = new OptionChain(DateTime.Now, new EquitySymbol("SPY"), 365, DateTime.Now.AddDays(2).Date.Add(Option.DefaultExpirationTime));
        }
    }
}