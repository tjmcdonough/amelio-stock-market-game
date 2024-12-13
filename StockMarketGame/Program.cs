using Scalar.AspNetCore;
using StockMarketGame;
using StockMarketGame.Api;
using StockMarketGame.Common;
using StockMarketGame.Database;
using StockMarketGame.Exceptions;
using StockMarketGame.Extensions;
using StockMarketGame.Interfaces;
using StockMarketGame.Services;
using StockMarketGame.Workers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<SimpleExceptionHandler>();

// Can easily be switched over to a real database without effecting the repository or service layers
builder.Services.AddSingleton<IDatabase, InMemoryDatabase>();
builder.Services.AddSingletonServices();

builder.Services.AddSingleton<IStartupTask>(provider => new StockInitializer(provider.GetRequiredService<IStockService>()));

builder.Services.AddHostedService<StockUpdateBackgroundService>();

WebApplication app = builder.Build();

IEnumerable<IStartupTask> startupTasks = app.Services.GetServices<IStartupTask>();
foreach (IStartupTask startupTask in startupTasks)
{
    await startupTask.ExecuteAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Amelio | Stock Market Game");
    });
}

/*
TODO: Add reflection service registration
*/

app.UseHttpsRedirection();
app.UseExceptionHandler("/oops");
app.RegisterStocksEndpoints();
app.RegisterPopularStockEndpoints();
app.RegisterSumStockEndpoints();

app.Run();