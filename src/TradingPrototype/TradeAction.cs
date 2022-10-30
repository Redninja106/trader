using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPrototype.Options;

namespace TradingPrototype;
internal class TradeAction
{
    public enum Action
    {
        Long,
        Short,
    }

    public TradeAction(DateTime asOf, Action action, EquitySymbol symbol, int quantity, decimal price)
    {
        this.TradeDate = asOf;
        this.OrderAction = action;
        this.Symbol = symbol;
        this.Quantity = quantity;
        this.Price = price;
    }
    public TradeAction(DateTime asOf, Action action, IOption option, int quantity, decimal price = 0)
    {
        this.Option = option;
        this.TradeDate = asOf;
        this.OrderAction = action;
        this.Quantity = quantity;
        this.Price = price == 0 ? (decimal)option.Price : price;
    }

    public DateTime TradeDate { get; private set; }
    public Action OrderAction { get; set; }
    public EquitySymbol Symbol { get; private set; }
    public IOption Option { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
}
