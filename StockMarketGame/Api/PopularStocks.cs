using StockMarketGame.Interfaces;

namespace StockMarketGame.Api;

public static class PopularStocksApi
{
    public static void RegisterPopularStockEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/popular-stocks", GetPopularStocks);
    }

    private static async Task<IResult> GetPopularStocks(IStockService stockGameService)
    {
        string stocks = await stockGameService.GetPopularStockNames(3);
        return Results.Ok(stocks);
    }
}