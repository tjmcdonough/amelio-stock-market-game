using StockMarketGame.Common;

namespace StockMarketGame.Interfaces;

public interface IStockMarket : ISingletonService
{
    long GetNextStockPrice(string name, long value);
}