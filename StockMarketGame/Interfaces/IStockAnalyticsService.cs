using StockMarketGame.Common;
using StockMarketGame.Services;

namespace StockMarketGame.Interfaces;

public interface IStockAnalyticsService : ISingletonService
{
    Task<SumAllStockResponse> SumAllStocks();
}