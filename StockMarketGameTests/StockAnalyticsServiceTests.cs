using Moq;
using StockMarketGame.Entities;
using StockMarketGame.Exceptions;
using StockMarketGame.Interfaces;
using StockMarketGame.Services;

namespace StockMarketGameTests;

public class StockAnalyticsServiceTests
{
    private readonly Mock<IStockRepository> _mockRepository;
    private readonly StockAnalyticsService _service;

    public StockAnalyticsServiceTests()
    {
        _mockRepository = new Mock<IStockRepository>();
        _service = new StockAnalyticsService(_mockRepository.Object);
    }

    [Fact]
    public async Task SumAllStocks_WithCompleteSet_ReturnsSumResponse()
    {
        // Arrange
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        List<Stock> stocks =
        [
            new("stock-1", 1000),
            new("stock-2", 1500),
            new("stock-3", 2000)
        ];
        
        stocks[0].SetUpdatedAt(timestamp);
        stocks[0].SetPopularity(5);
        stocks[1].SetUpdatedAt(timestamp);
        stocks[1].SetPopularity(3);
        stocks[2].SetPopularity(1);
        stocks[2].SetUpdatedAt(timestamp);

        _mockRepository.Setup(r => r.List())
            .ReturnsAsync(stocks);

        // Act
        SumAllStockResponse result = await _service.SumAllStocks();

        // Assert
        Assert.Equal(4500, result.Sum);
        Assert.Equal(timestamp, result.Timestamp);
        Assert.Equal(3, result.StockCount);
    }

    [Fact]
    public async Task SumAllStocks_WithIncompleteSet_ThrowsException()
    {
        // Arrange
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        List<Stock> stocks =
        [
            new("stock-1", 1000),
            new("stock-2", 1500),
            new("stock-3", 2000)
        ];
        
        stocks[0].SetUpdatedAt(timestamp);
        stocks[1].SetUpdatedAt(timestamp - 1);
        stocks[2].SetUpdatedAt(timestamp);
        
        _mockRepository.Setup(r => r.List())
            .ReturnsAsync(stocks);

        // Act & Assert
        await Assert.ThrowsAsync<NoCompleteStockSetException>(() => 
            _service.SumAllStocks());
    }
    
    [Fact]
    public async Task SumAllStocks_WithNegativeValues_ReturnsCorrectSum()
    {
        // Arrange
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        List<Stock> stocks =
        [
            new("stock-1", -1000), // Testing negative values
            new("stock-2", 1500),
            new("stock-3", 2000)
        ];
        
        foreach (var stock in stocks)
        {
            stock.SetUpdatedAt(timestamp);
            stock.SetPopularity(1);
        }

        _mockRepository.Setup(r => r.List())
            .ReturnsAsync(stocks);

        // Act
        SumAllStockResponse result = await _service.SumAllStocks();

        // Assert
        Assert.Equal(2500, result.Sum);
        Assert.Equal(timestamp, result.Timestamp);
        Assert.Equal(3, result.StockCount);
    }
}