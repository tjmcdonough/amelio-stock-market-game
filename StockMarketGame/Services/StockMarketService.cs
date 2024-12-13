using StockMarketGame.Interfaces;

namespace StockMarketGame.Services;

public class StockMarketService : IStockMarket
{
    private readonly Random _random = new();
    
    public long GetNextStockPrice(string name, long value)
    {
        Thread.Sleep(800);
        return long.Max(1, value + _random.Next(-10, 11));
    }
}