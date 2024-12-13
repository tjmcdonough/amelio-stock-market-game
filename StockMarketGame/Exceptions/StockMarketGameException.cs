namespace StockMarketGame.Exceptions;

public abstract class StockMarketGameException : Exception
{
    protected StockMarketGameException(string message) : base(message)
    {
    }

    protected StockMarketGameException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}