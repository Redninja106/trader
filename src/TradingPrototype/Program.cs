using System.Diagnostics;
using TradingPrototype;
using TradingPrototype.Technicals;

Version v = new Version(1, 0, 3, 0);
Version v1 = new Version(1, 0, 13, 0);
var spyData = DataSet.Load(Symbols.SPY, @".\RawData\1010_22to10212022_3min.csv");
spyData.AddTechnical(TechnicalAnalysis.Ema(8));
spyData.AddTechnical(TechnicalAnalysis.Ema(21));
spyData.AddTechnical(TechnicalAnalysis.AverageVolume(8));

spyData.EmulateRealTime();

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
    foreach (var transaction in trader.Transactions)
    {
        Console.WriteLine($"In @ {transaction.OpenTrade.TradeDate} Out @ {transaction.CloseTrade.TradeDate}, G/L:{transaction.Position.GainLoss:c},{transaction.Position.GainLossPercent:p2}");
        profits1 += transaction.Position.GainLoss;
    }
    Console.WriteLine($"trader1 profited ${profits1:c} in {trader.Transactions.Count} trades!!!");
}