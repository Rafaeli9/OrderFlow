using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Common;

namespace OrderFlow.Application.UseCases.ListProducts;

public sealed class ListProductsUseCase
{
    private readonly IProductRepository _repository;

    public ListProductsUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ProductDto>> ExecuteAsync(ListProductsQuery query, CancellationToken ct = default)
    {
        var result = await _repository.ListAsync(
            query.Search,
            query.Active,
            query.Page,
            query.PageSize,
            ct);

        var items = result.Items
            .Select(p => new ProductDto(p.Id, p.Name, p.Sku, p.Price, p.Active))
            .ToList();

        return new PagedResult<ProductDto>(items, result.Page, result.PageSize, result.TotalItems);
    }
}