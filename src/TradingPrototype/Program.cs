using System.Diagnostics;
using TradingPrototype;

var strategy1 = new TestStrategy(new AverageVolumeAnalyzer(50));
var strategy2 = new TestStrategy(new AverageVolumeAnalyzer(5));

var trader1 = new ConsoleTrader(strategy1);
var trader2 = new ConsoleTrader(strategy2);

var spyData = DataSet.Load(Symbols.SPY, "SPY.csv");
var vixData = DataSet.Load(Symbols.VIX, "^VIX.csv");

var marketRunner = new MarketRunner(new[] { spyData, vixData });

marketRunner.Run(new[] { trader1, trader2 });

float profits1 = 0;
foreach (var trade in trader1.ClosedTrades)
{
    profits1 += trade.GainLoss;
}
Console.WriteLine($"trader1 profited ${profits1} in {trader1.ClosedTrades.Count} trades!!!");

float profits2 = 0;
foreach (var trade in trader2.ClosedTrades)
{
    profits2 += trade.GainLoss;
}
Console.WriteLine($"trader2 profited ${profits2} in {trader2.ClosedTrades.Count} trades!!!");