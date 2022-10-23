using System.Diagnostics;
using TradingPrototype;
using TradingPrototype.Technicals;


var spyData = DataSet.Load(Symbols.SPY, @".\RawData\3Min1017_1021.csv");
spyData.AddTechnical(TechnicalAnalysis.Ema(8));
spyData.AddTechnical(TechnicalAnalysis.Ema(21));
spyData.AddTechnical(TechnicalAnalysis.AverageVolume(8));

var strategy1 = new TestStrategy(spyData);

List<ITrader> traders = new List<ITrader>()
{
    new ConsoleTrader(strategy1),
};

var marketRunner = new MarketRunner(new[] { spyData });

while(marketRunner.CanTrade)
{
    marketRunner.Run(traders.ToArray());
}

foreach (var trader in traders.OfType<ConsoleTrader>())
{
    decimal profits1 = 0;
    foreach (var trade in trader.ClosedTrades)
    {
        //Console.WriteLine($"In at {trade.OpenCandle.Timestamp} Cost: {trade.OpenCandle.Close} Out:{trade.CloseCandle.Close}");
        profits1 += trade.GainLoss;
    }
    Console.WriteLine($"trader1 profited ${profits1} in {trader.ClosedTrades.Count} trades!!!");
}