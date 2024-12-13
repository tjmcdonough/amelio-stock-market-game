using Moq;
using StockMarketGame.Entities;
using StockMarketGame.Exceptions;
using StockMarketGame.Interfaces;
using StockMarketGame.Services;

namespace StockMarketGameTests;

public class StocksServiceTests
{
    private readonly Mock<IStockRepository> _mockRepository;
    private readonly StocksService _service;

    public StocksServiceTests()
    {
        _mockRepository = new Mock<IStockRepository>();
        _service = new StocksService(_mockRepository.Object);
    }

    [Fact]
    public async Task AddStock_CallsRepository()
    {
        // Arrange
        Stock stock = new Stock("stock-1", 1000);
        _mockRepository.Setup(r => r.Add(It.IsAny<Stock>()))
            .ReturnsAsync(true);

        // Act
        bool result = await _service.AddStock(stock);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.Add(It.Is<Stock>(s => 
            s.Name == "stock-1" && 
            s.Price == 1000)), Times.Once);
    }

    [Fact]
    public async Task GetStock_ExistingStock_ReturnsPrice()
    {
        // Arrange
        Stock stock = new("stock-1", 1000);
        stock.SetPopularity(0);  // Use the proper method to set initial popularity
    
        _mockRepository.Setup(r => r.Get("stock-1"))
            .ReturnsAsync(stock);
        _mockRepository.Setup(r => r.Update(It.IsAny<Stock>()))
            .Returns(Task.FromResult(true));

        // Act
        long price = await _service.GetStock("stock-1");

        // Assert
        Assert.Equal(1000, price);
        _mockRepository.Verify(r => r.Update(It.Is<Stock>(s => 
            s.Popularity == 1)), Times.Once);
    }

    [Fact]
    public async Task GetStock_NonExistentStock_ThrowsException()
    {
        // Arrange
        _mockRepository.Setup(r => r.Get("non-existent"))
            .ReturnsAsync((Stock?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StockNotFoundException>(() => 
            _service.GetStock("non-existent"));
    }

    [Fact]
    public async Task AddStock_WithInvalidPrice_ThrowsArgumentException()
    {
        // Arrange
        Stock stock = new("stock-1", -1000); // Negative price

        // Act & Assert
        await Assert.ThrowsAsync<InvalidStockPriceException>(() => 
            _service.AddStock(stock));
    }
    
    [Fact]
    public async Task GetPopularStockNames_WithEmptyRepository_ReturnsEmptyString()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetPopularStocks(3))
            .ReturnsAsync(new List<Stock>());

        // Act
        string result = await _service.GetPopularStockNames(3);

        // Assert
        Assert.Equal("", result);
    }
    
    [Fact]
    public async Task GetPopularStockNames_WithLessThanThreeStocks_ReturnsAllAvailable()
    {
        // Arrange
        List<Stock> stocks =
        [
            new("stock-1", 1000),
            new("stock-2", 1000)
        ];

        stocks[0].SetPopularity(5);
        stocks[1].SetPopularity(3);
        
        _mockRepository.Setup(r => r.GetPopularStocks(3))
            .ReturnsAsync(stocks);

        // Act
        string result = await _service.GetPopularStockNames(3);

        // Assert
        Assert.Equal("stock-1,stock-2", result);
    }
}