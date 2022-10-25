

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
                new Tick(100m, 1),
                new Tick(110m, 1),
                new Tick(90m, 1),
                new Tick(105m, 1),
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
                new Tick(100m, 1),
                new Tick(110m, 1),
                new Tick(90m, 1),
                new Tick(105m, 1),
            };

            Candle c = Candle.FromTicks(new DateTime(2022, 10, 21, 9, 33, 00), ticks);

            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(105m, c.Close);
        }

        [TestMethod]
        public void CandleReplay04()
        {
            List<Tick> ticks = new()
            {
                new Tick(100m, 1), // open
                new Tick(107, 1), 
                new Tick(110m, 1), // high
                new Tick(92m, 1),
                new Tick(90m, 1), // low
                new Tick(105m, 1), // close
            };

            Candle c = Candle.Zero(new DateTime(2022, 10, 21, 9, 33, 00));

            Assert.AreEqual(new DateTime(2022, 10, 21, 9, 33, 00), c.Timestamp);
            Assert.AreEqual(0m, c.Open);
            Assert.AreEqual(0m, c.High);
            Assert.AreEqual(0m, c.Low);
            Assert.AreEqual(0m, c.Close);
            Assert.AreEqual(0m, c.Volume);

            c.Tick(ticks[0]); // The Open
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(100m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(100m, c.Close);
            Assert.AreEqual(1, c.Volume);

            c.Tick(ticks[1]);
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(107m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(107m, c.Close);
            Assert.AreEqual(2, c.Volume);

            c.Tick(ticks[2]);
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(100m, c.Low);
            Assert.AreEqual(110m, c.Close);
            Assert.AreEqual(3, c.Volume);

            c.Tick(ticks[3]);
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(92m, c.Low);
            Assert.AreEqual(92m, c.Close);
            Assert.AreEqual(4, c.Volume);

            c.Tick(ticks[4]);
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(90m, c.Close);
            Assert.AreEqual(5, c.Volume);

            c.Tick(ticks[5]);
            Assert.AreEqual(100m, c.Open);
            Assert.AreEqual(110m, c.High);
            Assert.AreEqual(90m, c.Low);
            Assert.AreEqual(105m, c.Close);
            Assert.AreEqual(6, c.Volume);
        }

    }
}