namespace StockMarketGame.Database;
public class Document
{
    public required string Id { get; init; }
    public required Dictionary<string, object> Data { get; init; }

    public Document()
    {
        Data = new Dictionary<string, object>();
    }
}