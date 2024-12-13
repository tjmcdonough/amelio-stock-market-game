namespace StockMarketGame.Exceptions;

public class InvalidStockPriceException(string collectionName, long price)
    : StockMarketGameException($"Price can't be negative '{price}' '{collectionName}'");