

using System.Security.Cryptography.X509Certificates;
using TradingPrototype;
using TradingPrototype.Options;
using TradingPrototype.Technicals;

namespace TradingPrototypeUnitTests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void PositionTest01Long()
        {
            FakeOption o = new FakeOption(OptionContractType.Put, 1.00d);
            FakeCandle c = new FakeCandle();

            TradeAction a = new TradeAction(DateTime.Now, TradeAction.Action.Long, o, 10, (decimal)o.Price);
            TradePosition p = new TradePosition(DateTime.Now, a, o);

            Assert.AreEqual(10, p.Quantity);
            Assert.AreEqual(1000, p.TotalCost);
            Assert.AreEqual(1000, p.TotalValue);
            Assert.AreEqual(0, p.GainLoss);
            Assert.AreEqual(0m, p.GainLossPercent);

            o = new FakeOption(OptionContractType.Put, 1.50d);
            p = new TradePosition(DateTime.Now, a, o);
            Assert.AreEqual(10, p.Quantity);
            Assert.AreEqual(1000, p.TotalCost);
            Assert.AreEqual(1500, p.TotalValue);
            Assert.AreEqual(500, p.GainLoss);
            Assert.AreEqual(.5m, p.GainLossPercent);

            o = new FakeOption(OptionContractType.Put, .50d);
            p = new TradePosition(DateTime.Now, a, o);
            Assert.AreEqual(10, p.Quantity);
            Assert.AreEqual(1000, p.TotalCost);
            Assert.AreEqual(500, p.TotalValue);
            Assert.AreEqual(-500, p.GainLoss);
            Assert.AreEqual(-.5m, p.GainLossPercent);
        }

        [TestMethod]
        public void PositionTest01Short()
        {
            FakeOption o = new FakeOption(OptionContractType.Put, 1.00d);
            FakeCandle c = new FakeCandle();

            TradeAction a = new TradeAction(DateTime.Now, TradeAction.Action.Short, o, 10, (decimal)o.Price);
            TradePosition p = new TradePosition(DateTime.Now, a, o);

            Assert.AreEqual(-10, p.Quantity);
            Assert.AreEqual(-1000, p.TotalCost);
            Assert.AreEqual(-1000, p.TotalValue);
            Assert.AreEqual(0, p.GainLoss);
            Assert.AreEqual(0m, p.GainLossPercent);

            o = new FakeOption(OptionContractType.Put, 1.50d);
            p = new TradePosition(DateTime.Now, a, o);
            Assert.AreEqual(-10, p.Quantity);
            Assert.AreEqual(-1000, p.TotalCost);
            Assert.AreEqual(-1500, p.TotalValue);
            Assert.AreEqual(-500, p.GainLoss);
            Assert.AreEqual(-.5m, p.GainLossPercent);

            o = new FakeOption(OptionContractType.Put, .50d);
            p = new TradePosition(DateTime.Now, a, o);
            Assert.AreEqual(-10, p.Quantity);
            Assert.AreEqual(-1000, p.TotalCost);
            Assert.AreEqual(-500, p.TotalValue);
            Assert.AreEqual(500, p.GainLoss);
            Assert.AreEqual(.5m, p.GainLossPercent);
        }

        class FakeCandle : ICandle
        {
            public FakeCandle()
            {
                this.Timestamp = DateTime.Now;
            }

            public decimal Close { get; private set; }
            public decimal High { get; private set; }
            public decimal Low { get; private set; }
            public decimal Open { get; private set; }
            public DateTime Timestamp { get; private set; }
            public long Volume { get; private set; }
        }

        class FakeOption : IOption
        {
            public FakeOption(OptionContractType type, double price)
            {
                this.UnderlyingSymbol =new EquitySymbol("SPY");
                this.Type = type;
                this.Price= price;
            }

            public double Delta { get; set; }
            public DateTimeOffset Expiration { get; set; }
            public double Gamma { get; set; }
            public double Price { get; set; }
            public double Rho { get; set; }
            public double StrikePrice { get; set; }
            public string Symbol { get; set; }
            public double Theta { get; set; }
            public OptionContractType Type { get; set; }
            public EquitySymbol UnderlyingSymbol { get; set; }
            public double Vega { get; set; }
        }
    }
}