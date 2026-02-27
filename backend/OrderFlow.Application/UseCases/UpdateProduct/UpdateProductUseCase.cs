using OrderFlow.Application.Abstractions;

namespace OrderFlow.Application.UseCases.UpdateProduct;

public sealed class UpdateProductUseCase
{
    private readonly IProductRepository _repository;

    public UpdateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(Guid id, UpdateProductRequest request, CancellationToken ct = default)
    {
        var product = await _repository.GetByIdForUpdateAsync(id, ct);

        if (product is null)
            return false;
            
        var normalizedSku = request.Sku.Trim().ToUpperInvariant();
        if (!string.Equals(product.Sku, normalizedSku, StringComparison.Ordinal))
        {
            var existing = await _repository.GetBySkuAsync(normalizedSku, ct);
            if (existing is not null)
                throw new InvalidOperationException("Product with this SKU already exists.");
        }

        product.SetName(request.Name);
        product.SetSku(request.Sku);
        product.SetPrice(request.Price);

        if (request.Active) product.Activate();
        else product.Deactivate();

        await _repository.SaveChangesAsync(ct);
        return true;
    }
}