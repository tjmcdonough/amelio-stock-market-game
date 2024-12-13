using StockMarketGame.Entities;
using StockMarketGame.Exceptions;
using StockMarketGame.Interfaces;

namespace StockMarketGame.Services;

public class StocksService(IStockRepository repository) : IStockService
{
    public async Task<bool> AddStock(Stock stock)
    {
        if(stock.Price <= 0)
            throw new InvalidStockPriceException(nameof(Stock), stock.Price);
        return await repository.Add(stock);
    }

    public async Task<long> GetStock(string id)
    {
        Stock? stock = await repository.Get(id);
        if (stock is null)
            throw new StockNotFoundException(id);
        
        stock.SetPopularity(stock.Popularity + 1);

        await repository.Update(stock);

        return stock.Price;
    }
    
    public Task<IEnumerable<Stock>> GetPopularStocks(int limit = 10)
    {
        return repository.GetPopularStocks(limit);
    }
    
    public async Task<string> GetPopularStockNames(int limit = 10)
    {
        IEnumerable<Stock> stocks = await GetPopularStocks(3);

        return string.Join(",", stocks.Select(s => s.Name));
    }
}