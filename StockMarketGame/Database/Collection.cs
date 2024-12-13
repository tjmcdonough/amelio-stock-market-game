using System.Collections.Concurrent;

namespace StockMarketGame.Database;

public class Collection
{
    private readonly ConcurrentDictionary<string, Document> _documents;

    public Collection()
    {
        _documents = new ConcurrentDictionary<string, Document>();
    }

    public bool InsertOne(Document document)
    {
        return _documents.TryAdd(document.Id, document);
    }

    public Document? FindOne(string id)
    {
        _documents.TryGetValue(id, out Document? document);
        return document;
    }

    public IEnumerable<Document> Find(Func<Document, bool> filter)
    {
        return _documents.Values.Where(filter);
    }

    public bool UpdateOne(string id, Dictionary<string, object> updates)
    {
        if (!_documents.TryGetValue(id, out Document? document)) return false;
            
        foreach (KeyValuePair<string, object> update in updates)
        {
            document.Data[update.Key] = update.Value;
        }

        return true;
    }

    public bool DeleteOne(string id)
    {
        return _documents.TryRemove(id, out _);
    }
}