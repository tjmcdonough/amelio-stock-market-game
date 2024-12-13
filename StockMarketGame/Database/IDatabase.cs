namespace StockMarketGame.Database;

public interface IDatabase
{
    Task<bool> Insert<T>(string collectionName, string id, T item);
    Task<T?> GetById<T>(string collectionName, string id) where T : class;
    Task<IEnumerable<T>> FindAll<T>(string collectionName) where T : class;
    Task<bool> Update<T>(string collectionName, string id, T item);
    Task<bool> Delete(string collectionName, string id);

    Task<IEnumerable<T>> GetTopByField<T>(
        string collectionName,
        string fieldName,
        int limit,
        bool descending = true) where T : class;
}