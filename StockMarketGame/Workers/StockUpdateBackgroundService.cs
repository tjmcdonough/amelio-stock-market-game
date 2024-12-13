using StockMarketGame.Entities;
using StockMarketGame.Interfaces;

namespace StockMarketGame.Workers;

public class StockUpdateBackgroundService(
    IStockRepository stockRepository,
    IStockMarket stockMarket,
    ILogger<StockUpdateBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount * 2,
            CancellationToken = stoppingToken
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            long batchCreatedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            IEnumerable<Stock> stocks = await stockRepository.List();

            await Parallel.ForEachAsync(stocks, parallelOptions, async (stock, _) =>
            {
                try
                {
                    long newPrice = stockMarket.GetNextStockPrice(stock.Name, stock.Price);
                    stock.SetPrice(newPrice);
                    stock.SetUpdatedAt(batchCreatedDate);
                    await stockRepository.Update(stock);
                    logger.LogInformation("Stock {StockName}; Price {Price}; Date: {Date}", 
                        stock.Name, newPrice, batchCreatedDate);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing stock {StockName}", stock.Name);
                }
            });
        }
    }
}