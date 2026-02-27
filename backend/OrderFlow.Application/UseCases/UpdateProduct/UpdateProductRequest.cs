namespace OrderFlow.Application.UseCases.UpdateProduct;

public sealed record UpdateProductRequest(
    string Name,
    string Sku,
    decimal Price,
    bool Active
);