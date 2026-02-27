using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Common;
using OrderFlow.Domain.Entities;

namespace OrderFlow.UnitTests.Fakes;

public sealed class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
    {
        var normalized = sku.Trim().ToUpperInvariant();
        var product = _products.FirstOrDefault(p => p.Sku == normalized);
        return Task.FromResult(product);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<Product?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task AddAsync(Product product, CancellationToken ct = default)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => Task.CompletedTask;

    public Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct = default)
        => Task.FromResult((IReadOnlyList<Product>)_products.ToList());

    public Task<PagedResult<Product>> ListAsync(string? search, bool? active, int page, int pageSize, CancellationToken ct = default)
    {
        IEnumerable<Product> query = _products;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToUpperInvariant();
            query = query.Where(p =>
                p.Name.ToUpperInvariant().Contains(s) ||
                p.Sku.ToUpperInvariant().Contains(s));
        }

        if (active.HasValue)
            query = query.Where(p => p.Active == active.Value);

        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var total = query.Count();
        var items = query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(new PagedResult<Product>(items, page, pageSize, total));
    }
}