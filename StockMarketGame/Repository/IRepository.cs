using StockMarketGame.Common;
using StockMarketGame.Interfaces;

namespace StockMarketGame.Repository;

public interface IRepository<T> : ISingletonService 
    where T : class, IEntity
{
    Task<bool> Add(T item);
    Task<T?> Get(string name);
    Task<IEnumerable<T>> List();
    Task<bool> Update(T item);
    Task<bool> Delete(string name);
}