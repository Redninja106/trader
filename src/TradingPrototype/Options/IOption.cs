namespace TradingPrototype.Options
{
    internal interface IOption
    {
        double Delta { get; }
        DateTimeOffset Expiration { get; }
        double Gamma { get; }
        double Price { get; }
        double Rho { get; }
        double StrikePrice { get; }
        string Symbol { get; }
        double Theta { get; }
        OptionContractType Type { get; }
        EquitySymbol UnderlyingSymbol { get; }
        double Vega { get; }
    }
}