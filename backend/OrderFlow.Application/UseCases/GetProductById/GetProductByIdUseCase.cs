using OrderFlow.Application.Abstractions;
using OrderFlow.Application.UseCases.ListProducts;

namespace OrderFlow.Application.UseCases.GetProductById;

public sealed class GetProductByIdUseCase
{
    private readonly IProductRepository _repository;

    public GetProductByIdUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto?> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _repository.GetByIdAsync(id, ct);

        if (product is null)
            return null;

        return new ProductDto(
            product.Id,
            product.Name,
            product.Sku,
            product.Price,
            product.Active);
    }
}