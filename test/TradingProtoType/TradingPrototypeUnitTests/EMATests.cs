

using TradingPrototype;
using TradingPrototype.Technicals;

namespace TradingPrototypeUnitTests
{
    [TestClass]
    public class EMATests
    {
        [TestMethod]
        public void TestEma()
        {
            string path = @".\Data\3min1017_1021.csv";
            DataSet ds = DataSet.Load("Test", path);

            var checkData = System.IO.File.ReadAllLines(path).Skip(1).Select(l => l.Split(','))
                .Select(parts => new
                {
                    TimeStamp = DateTime.Parse(parts[0] + " " + parts[1]),
                    ema8 = float.Parse(parts[7]),
                    ema21 = float.Parse(parts[8]),
                });

            //EMAAnalyzer ema8 = new EMAAnalyzer(ds, 8);
            //EMAAnalyzer ema21 = new EMAAnalyzer(ds, 21);

            ds.Candles.ForEach(c => c.TickAll());

            while(!ds.IsAtEnd)
            {
                var checkDataItem = checkData.Single(d => d.TimeStamp == ds.CurrentCandle.Timestamp);
                //ema8.Calculate(ds);
                //ema21.Calculate(ds);

                Assert.AreEqual(checkDataItem.ema8, ds.Ema(8));
                Assert.AreEqual(checkDataItem.ema21, ds.Ema(21));

                ds.Advance();
            }

        }
    }
}