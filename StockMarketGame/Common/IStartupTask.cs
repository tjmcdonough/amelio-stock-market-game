namespace StockMarketGame.Common;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}