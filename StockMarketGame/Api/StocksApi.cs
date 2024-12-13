using StockMarketGame.Entities;
using StockMarketGame.Interfaces;
using StockMarketGame.Requests;

namespace StockMarketGame.Api;

public static class StocksApi
{
    public static void RegisterStocksEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/stocks", GetStock);
        
        endpointRouteBuilder.MapPost("/stocks", AddStock);
    }

    private static async Task<IResult> GetStock(string name, IStockService service)
    {
        long stockPrice = await service.GetStock(name);
        return Results.Ok(stockPrice);
    }
    
    private static async Task<IResult> AddStock(StockRequest request, IStockService service)
    {
        Stock stock = new(request.Name, 1000);
        bool valid = await service.AddStock(stock);
        return Results.Ok(valid);
    }
}