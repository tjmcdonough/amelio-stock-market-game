using StockMarketGame.Interfaces;
using StockMarketGame.Services;

namespace StockMarketGame.Api;

public static class SumStocksApi
{
    public static void RegisterSumStockEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/sum-stocks", SumStocks);
    }

    private static async Task<IResult> SumStocks(IStockAnalyticsService analyticsService)
    {
        SumAllStockResponse sum = await analyticsService.SumAllStocks();
        string response = $"Sum of all stocks: {sum.Sum}, Stock Count: {sum.StockCount}, Timestamp: {sum.Timestamp}";
        return Results.Ok(response);
    }
}