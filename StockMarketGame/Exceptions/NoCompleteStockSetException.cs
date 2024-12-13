namespace StockMarketGame.Exceptions;

public class NoCompleteStockSetException(
    long timestamp)
    : StockMarketGameException($"No complete set of stock values found. Latest timestamp: {timestamp}");