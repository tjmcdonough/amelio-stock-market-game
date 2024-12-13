using StockMarketGame.Database;
using StockMarketGame.Entities;
using StockMarketGame.Interfaces;

namespace StockMarketGame.Repository;

public class StockRepository(IDatabase database) : IStockRepository
{
    private const string CollectionName = "stocks";

    public Task<bool> Add(Stock stock)
        => database.Insert(CollectionName, stock.Name, stock);

    public Task<Stock?> Get(string name)
        => database.GetById<Stock>(CollectionName, name);

    public Task<IEnumerable<Stock>> List()
        => database.FindAll<Stock>(CollectionName);

    public Task<bool> Update(Stock stock)
        => database.Update(CollectionName, stock.Name, stock);

    public Task<bool> Delete(string name)
        => database.Delete(CollectionName, name);
    
    public async Task<IEnumerable<Stock>> GetPopularStocks(int limit = 10)
    {
        IEnumerable<Stock> allStocks = await database.GetTopByField<Stock>(CollectionName, "Popularity", limit);
        return allStocks
            .OrderByDescending(s => s.Popularity)
            .Take(limit);
    }
}