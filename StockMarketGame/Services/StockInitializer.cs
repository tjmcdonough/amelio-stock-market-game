using StockMarketGame.Common;
using StockMarketGame.Entities;
using StockMarketGame.Interfaces;

namespace StockMarketGame.Services;

public class StockInitializer : IStartupTask
{
    private readonly IStockService _stockService;
    
    private readonly Dictionary<string, int> _initialStocks = Enumerable.Range(1, 10)
        .ToDictionary(
            i => $"stock-{i}",
            _ => 1000
        );

    public StockInitializer(IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        foreach (Stock stock in _initialStocks.Select(initialStock =>
                     new Stock(initialStock.Key, initialStock.Value)))
        {
            await _stockService.AddStock(stock);
        }
    }
}