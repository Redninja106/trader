using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPrototype;
internal class PositionedDataSetView : IDataSetView
{
    private DataSet dataSet;
    private int currentIndex;

    public PositionedDataSetView(DataSet dataSet)
    {
        this.dataSet = dataSet;
    }

    public bool IsAtEnd => currentIndex >= dataSet.Candles.Count;

    public Candle MostRecent => dataSet.Candles[currentIndex];

    public IEnumerable<Candle> LookBack(int count)
    {
        for (int i = currentIndex - count; i < currentIndex; i++)
        {
            if (i < 0)
                continue;

            yield return dataSet.Candles[i];
        }
    }

    public void Advance()
    {
        currentIndex++;
    }
}
