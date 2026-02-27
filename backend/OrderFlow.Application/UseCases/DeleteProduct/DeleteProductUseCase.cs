using OrderFlow.Application.Abstractions;

namespace OrderFlow.Application.UseCases.DeleteProduct;

public sealed class DeleteProductUseCase
{
    private readonly IProductRepository _repository;

    public DeleteProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _repository.GetByIdForUpdateAsync(id, ct);

        if (product is null)
            return false;

        product.Deactivate();
        await _repository.SaveChangesAsync(ct);

        return true;
    }
}