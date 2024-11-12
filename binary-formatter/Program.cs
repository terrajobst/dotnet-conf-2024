internal class Program
{
    private static void Main(string[] args)
    {
        var order = new Order(42, [
            new OrderItem(new Product(2, "No Name Chocolate Bar"), 2, 1.85m),
            new OrderItem(new Product(3, "No Name Diet Pop "), 3, 2.90m),
        ]);
        byte[] bytes = Serialize(order);
        string text = Convert.ToBase64String(bytes);
        Console.WriteLine(text);
    }

    private static byte[] Serialize(object data)
    {
        using var stream = new MemoryStream();

        // TODO: Serialize

        return stream.ToArray();
    }
}

#region Types

[Serializable]
class Order
{
    public Order(int id, IEnumerable<OrderItem> items)
    {
        Id = id;
        Items.AddRange(items);
    }

    public int Id { get; }
    public List<OrderItem> Items { get; } = new();
}

[Serializable]
class OrderItem
{
    public OrderItem(Product product, int quanity, decimal price)
    {
        Product = product;
        Quanity = quanity;
        Price = price;
    }

    public Product Product { get; }
    public int Quanity { get; }
    public decimal Price { get; }
}

[Serializable]
class Product
{
    public Product(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }
}

#endregion
