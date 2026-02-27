namespace OrderFlow.Application.UseCases.ListProducts;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    bool Active
);