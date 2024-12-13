namespace StockMarketGame.Exceptions;

public class StockNotFoundException(string stockId) 
    : StockMarketGameException($"Stock with ID {stockId} was not found");