using OrderFlow.Domain.Entities;
using OrderFlow.Application.Common;

namespace OrderFlow.Application.Abstractions;

public interface IProductRepository
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct = default);
    Task<PagedResult<Product>> ListAsync(
        string? search,
        bool? active,
        int page,
        int pageSize,
        CancellationToken ct = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);    

    Task<Product?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);  
}
