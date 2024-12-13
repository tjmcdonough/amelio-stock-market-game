namespace StockMarketGame.Exceptions;

public class DuplicateKeyException(string collectionName, string id)
    : StockMarketGameException($"An item with id '{id}' already exists in collection '{collectionName}'");