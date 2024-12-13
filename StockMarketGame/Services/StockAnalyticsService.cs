using StockMarketGame.Entities;
using StockMarketGame.Exceptions;
using StockMarketGame.Interfaces;

namespace StockMarketGame.Services;

public record SumAllStockResponse(long Sum, long Timestamp, int StockCount);

public class StockAnalyticsService(IStockRepository repository) : IStockAnalyticsService
{
    public async Task<SumAllStockResponse> SumAllStocks()
    {
        List<Stock> stocks = (await repository.List()).ToList();
    
        List<IGrouping<long, Stock>> stocksByTimestamp = stocks
            .GroupBy(s => s.UpdatedAt)
            .OrderByDescending(g => g.Key).ToList();
    
        foreach (IGrouping<long, Stock> group in stocksByTimestamp)
        {
            List<Stock> stocksInTimestamp = group.ToList();
            if (stocksInTimestamp.Count == stocks.Count)
            {
                return new SumAllStockResponse(
                    Sum: stocksInTimestamp.Sum(s => s.Price),
                    Timestamp: group.Key,
                    StockCount: stocksInTimestamp.Count
                );
            }
        }
    
        IGrouping<long, Stock>? latestGroup = stocksByTimestamp.FirstOrDefault();
        throw new NoCompleteStockSetException(
            timestamp: latestGroup?.Key ?? 0
        );
    }
}