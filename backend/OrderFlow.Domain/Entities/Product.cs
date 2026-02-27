namespace OrderFlow.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool Active { get; private set; } = true;

    private Product() { }

    public Product(string name, string sku, decimal price)
    {
        SetName(name);
        SetSku(sku);
        SetPrice(price);
    }

    public void Activate() => Active = true;
    public void Deactivate() => Active = false;

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        Name = name.Trim();
    }

    public void SetSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("Sku is required.");

        Sku = sku.Trim().ToUpperInvariant();
    }

    public void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        Price = price;
    }
}
