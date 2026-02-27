namespace OrderFlow.Application.UseCases.ListProducts;

public sealed record ListProductsQuery(
    string? Search,
    bool? Active,
    int Page = 1,
    int PageSize = 10
);