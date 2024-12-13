using StockMarketGame.Repository;

namespace StockMarketGame.Entities;

public class Stock : IEntity
{
    public string Name { get; internal init; }
    public long Price { get; private set; }
    public long UpdatedAt { get; private set; }
    public int Popularity { get; private set; }

    public Stock(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public void SetPrice(long value)
    {
        if (value <= 0) 
            throw new ArgumentException("Price must be positive");
        Price = value;
    }

    public void SetUpdatedAt(long value)
    {
        UpdatedAt = value;
    }

    public void SetPopularity(int value)
    {
        Popularity = value;
    }
}