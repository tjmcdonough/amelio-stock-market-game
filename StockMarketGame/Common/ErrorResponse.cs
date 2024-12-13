namespace StockMarketGame.Common;

public record ErrorResponse(string Message, string Code, DateTime Timestamp)
{
    public static ErrorResponse FromException(Exception ex, string code) => 
        new(ex.Message, code, DateTime.UtcNow);
}