using System.Collections.Concurrent;
using System.Reflection;
using StockMarketGame.Exceptions;

namespace StockMarketGame.Database;

public class InMemoryDatabase : IDatabase
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _collections = new();

    public Task<bool> Insert<T>(string collectionName, string id, T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        ConcurrentDictionary<string, object> collection =
            _collections.GetOrAdd(collectionName, _ => new ConcurrentDictionary<string, object>());

        if (!collection.TryAdd(id, item))
            throw new DuplicateKeyException(collectionName, id);

        return Task.FromResult(true);
    }

    public Task<T?> GetById<T>(string collectionName, string id) where T : class
    {
        if (_collections.TryGetValue(collectionName, out ConcurrentDictionary<string, object>? collection) &&
            collection.TryGetValue(id, out object? item) &&
            item is T typedItem)
        {
            return Task.FromResult<T?>(typedItem);
        }

        return Task.FromResult<T?>(null);
    }

    public Task<IEnumerable<T>> FindAll<T>(string collectionName) where T : class
    {
        return Task.FromResult(
            _collections.TryGetValue(collectionName, out ConcurrentDictionary<string, object>? collection)
                ? collection.Values.OfType<T>()
                : []);
    }

    public Task<bool> Update<T>(string collectionName, string id, T item)
    {
        if (!_collections.TryGetValue(collectionName, out ConcurrentDictionary<string, object>? collection))
            return Task.FromResult(false);

        if (item != null)
            collection[id] = item;

        return Task.FromResult(true);
    }

    public Task<bool> Delete(string collectionName, string id)
    {
        return Task.FromResult(
            _collections.TryGetValue(collectionName, out ConcurrentDictionary<string, object>? collection)
            && collection.TryRemove(id, out _));
    }

    public Task<IEnumerable<T>> FindByField<T>(string collectionName, string fieldName, object fieldValue) where T : class
    {
        if (!_collections.TryGetValue(collectionName, out ConcurrentDictionary<string, object>? collection))
            return Task.FromResult(Enumerable.Empty<T>());
        
        IEnumerable<T> results = collection.Values
            .OfType<T>()
            .Where(item => 
            {
                PropertyInfo? property = typeof(T).GetProperty(fieldName);
                if (property == null) return false;
                    
                object? value = property.GetValue(item);
                return value?.Equals(fieldValue) ?? false;
            });

        return Task.FromResult(results);
    }
    
    public Task<IEnumerable<T>> GetTopByField<T>(
        string collectionName, 
        string fieldName, 
        int limit, 
        bool descending = true) where T : class
    {
        if (!_collections.TryGetValue(collectionName, out ConcurrentDictionary<string, object>? collection))
            return Task.FromResult(Enumerable.Empty<T>());

        PropertyInfo? property = typeof(T).GetProperty(fieldName);
        if (property == null)
            throw new ArgumentException($"Field {fieldName} not found on type {typeof(T).Name}");

        IEnumerable<T> items = collection.Values.OfType<T>();
    
        IOrderedEnumerable<T> orderedItems = descending
            ? items.OrderByDescending(item => property.GetValue(item))
            : items.OrderBy(item => property.GetValue(item));

        return Task.FromResult(orderedItems.Take(limit));
    }
}