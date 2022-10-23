

using TradingPrototype;
using TradingPrototype.Technicals;

namespace TradingPrototypeUnitTests
{
    [TestClass]
    public class CandleTests
    {
        [TestMethod]
        public void CandleReplay01()
        {
            Candle c = new Candle(new DateTime(2022, 10, 21, 9, 33, 00), 100m, 110m, 90m, 105m, 9999);

            Assert.AreEqual(new DateTime(2022, 10, 21, 9, 33, 00), c.Timestamp);

            Assert.AreEqual(0m, c.Open);
            Assert.AreEqual(0m, c.High);
            Assert.AreEqual(0m, c.Low);
            Assert.AreEqual(0m, c.Close);

            c.TickAll();

            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(105m, c.Close);
        }

        [TestMethod]
        public void CandleReplay02()
        {
            List<Tick> ticks = new()
            {
                new Tick() { Price = 100m, Volume = 1 },
                new Tick() { Price = 110m, Volume = 1 },
                new Tick() { Price = 90m, Volume = 1 },
                new Tick() { Price = 105m, Volume = 1 },
            };

            Candle c = Candle.FromTicks(new DateTime(2022, 10, 21, 9, 33, 00), ticks);

            Assert.AreEqual(new DateTime(2022, 10, 21, 9, 33, 00), c.Timestamp);
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(105m, c.Close);
        }

        [TestMethod]
        public void CandleReplay03()
        {
            List<Tick> ticks = new()
            {
                new Tick() { Price = 100m, Volume = 1 },
                new Tick() { Price = 110m, Volume = 1 },
                new Tick() { Price = 90m, Volume = 1 },
                new Tick() { Price = 105m, Volume = 1 },
            };

            Candle c = new Candle(new DateTime(2022, 10, 21, 9, 33, 00), ticks);

            Assert.AreEqual(new DateTime(2022, 10, 21, 9, 33, 00), c.Timestamp);
            Assert.AreEqual(0m, c.Open);
            Assert.AreEqual(0m, c.High);
            Assert.AreEqual(0m, c.Low);
            Assert.AreEqual(0m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(100m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(100m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(110m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(90m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(105m, c.Close);

            Assert.IsFalse(c.MoreTicks);
        }

        [TestMethod]
        public void CandleReplay04()
        {
            List<Tick> ticks = new()
            {
                new Tick() { Price = 100m, Volume = 1 }, // open
                new Tick() { Price = 107, Volume = 1 }, 
                new Tick() { Price = 110m, Volume = 1 }, // high
                new Tick() { Price = 92m, Volume = 1 },
                new Tick() { Price = 90m, Volume = 1 }, // low
                new Tick() { Price = 105m, Volume = 1 }, // close
            };

            Candle c = new Candle(new DateTime(2022, 10, 21, 9, 33, 00), ticks);

            Assert.AreEqual(new DateTime(2022, 10, 21, 9, 33, 00), c.Timestamp);
            Assert.AreEqual(0m, c.Open);
            Assert.AreEqual(0m, c.High);
            Assert.AreEqual(0m, c.Low);
            Assert.AreEqual(0m, c.Close);

            c.Tick(); // The Open
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(100m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(100m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(107m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(107m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(110m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(92m, c.Low);
            Assert.AreEqual(92m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(90m, c.Close);

            c.Tick();
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(105m, c.Close);

            Assert.IsFalse(c.MoreTicks);
        }

    }
}