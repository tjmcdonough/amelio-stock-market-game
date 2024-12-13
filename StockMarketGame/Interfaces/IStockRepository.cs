using StockMarketGame.Entities;
using StockMarketGame.Repository;

namespace StockMarketGame.Interfaces;

public interface IStockRepository : IRepository<Stock>
{
    Task<IEnumerable<Stock>> GetPopularStocks(int limit = 10);
}