using OrderFlow.Application.Abstractions;

namespace OrderFlow.Application.UseCases.ActivateProduct;

public sealed class ActivateProductUseCase
{
    private readonly IProductRepository _repository;

    public ActivateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _repository.GetByIdForUpdateAsync(id, ct);

        if (product is null)
            return false;

        product.Activate();
        await _repository.SaveChangesAsync(ct);

        return true;
    }
}