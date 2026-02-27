using OrderFlow.Application.Abstractions;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.UseCases.CreateProduct;

public sealed class CreateProductUseCase
{
    private readonly IProductRepository _repository;

    public CreateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> ExecuteAsync(string name, string sku, decimal price, CancellationToken ct = default)
    {
        var existing = await _repository.GetBySkuAsync(sku, ct);

        if (existing is not null)
            throw new InvalidOperationException("Product with this SKU already exists.");

        var product = new Product(name, sku, price);

        await _repository.AddAsync(product, ct);

        return product.Id;
    }
}
