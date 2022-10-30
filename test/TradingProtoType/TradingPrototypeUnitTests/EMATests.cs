

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
            ds.AddTechnical(TechnicalAnalysis.Ema(8));
            //ds.AddTechnical(TechnicalAnalysis.Ema(21));

            var checkData = System.IO.File.ReadAllLines(path).Skip(1).Select(l => l.Split(','))
                .Select(parts => new
                {
                    TimeStamp = DateTime.Parse(parts[0] + " " + parts[1]),
                    ema8 = float.Parse(parts[7]),
                    ema21 = float.Parse(parts[8]),
                });

            while(!ds.IsAtEnd)
            {
                Assert.IsFalse(ds.CurrentCandleHasMoreTicks);
                var checkDataItem = checkData.Single(d => d.TimeStamp == ds.CurrentCandle.Timestamp);

                Assert.AreEqual(checkDataItem.ema8, ds.GetTechnicalIndicator<TechnicalResult.Ema>(TechnicalAnalysis.Ema(8)).Result);
              //  Assert.AreEqual(checkDataItem.ema21, ds.GetTechnicalIndicator<float>(TechnicalAnalysis.Ema(21)));

                ds.AdvanceCandle();
            }

        }
    }
}