using Moq;
using StockMarketGame.Entities;
using StockMarketGame.Interfaces;
using StockMarketGame.Services;

namespace StockMarketGameTests;

public class StockInitializerTests
{
    private readonly Mock<IStockService> _mockStockService;
    private readonly StockInitializer _initializer;

    public StockInitializerTests()
    {
        _mockStockService = new Mock<IStockService>();
        _initializer = new StockInitializer(_mockStockService.Object);
    }

    [Fact]
    public async Task ExecuteAsync_InitializesTenStocks()
    {
        // Arrange
        _mockStockService.Setup(s => s.AddStock(It.IsAny<Stock>()))
            .ReturnsAsync(true);

        // Act
        await _initializer.ExecuteAsync();

        // Assert
        _mockStockService.Verify(s => s.AddStock(It.IsAny<Stock>()), Times.Exactly(10));
        
        // Verify each stock was initialized correctly
        for (int i = 1; i <= 10; i++)
        {
            _mockStockService.Verify(s => s.AddStock(It.Is<Stock>(stock => 
                stock.Name == $"stock-{i}" && 
                stock.Price == 1000)), Times.Once);
        }
    }
}