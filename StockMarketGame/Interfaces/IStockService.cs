using StockMarketGame.Common;
using StockMarketGame.Entities;

namespace StockMarketGame.Interfaces;

public interface IStockService : ISingletonService
{
    Task<bool> AddStock(Stock stock);
    Task<long> GetStock(string name);
    Task<IEnumerable<Stock>> GetPopularStocks(int limit = 10);
    Task<string> GetPopularStockNames(int limit = 10);
}