namespace TradingPrototype.Technicals
{
    public class TechnicalResult<T> : TechnicalResult
    {
        public TechnicalResult(T result)
        {
            this.Result = result;
        }

        public T Result { get; }
    }

    public class TechnicalResult
    {
        public class Ema : TechnicalResult<float>
        {
            public Ema(float value) : base(value) { }
        }
        public class Sma : TechnicalResult<float>
        {
            public Sma(float value) : base(value) { }
        }

        public class StochasticFast : TechnicalResult<(float K, float D)>
        {
            public StochasticFast(float k, float d) : base(new(k, d)) { }
        }
    }
}
