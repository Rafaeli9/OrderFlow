using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Abstractions;
using OrderFlow.Application.UseCases.CreateProduct;
using OrderFlow.Application.UseCases.ListProducts;
using OrderFlow.Application.UseCases.GetProductById;
using OrderFlow.Application.UseCases.UpdateProduct;
using OrderFlow.Application.UseCases.DeleteProduct;


namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly CreateProductUseCase _createProductUseCase;
    private readonly ListProductsUseCase _listProductsUseCase;
    private readonly GetProductByIdUseCase _getProductByIdUseCase;
    private readonly UpdateProductUseCase _updateProductUseCase;
    private readonly DeleteProductUseCase _deleteProductUseCase;

 public ProductsController(
    CreateProductUseCase createProductUseCase,
    ListProductsUseCase listProductsUseCase,
    GetProductByIdUseCase getProductByIdUseCase,
    UpdateProductUseCase updateProductUseCase,
    DeleteProductUseCase deleteProductUseCase)
    {
        _createProductUseCase = createProductUseCase;
        _listProductsUseCase = listProductsUseCase;
        _getProductByIdUseCase = getProductByIdUseCase;
        _updateProductUseCase = updateProductUseCase;
        _deleteProductUseCase = deleteProductUseCase;
    }

  [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] bool? active,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var query = new ListProductsQuery(search, active, page, pageSize);
        var result = await _listProductsUseCase.ExecuteAsync(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var product = await _getProductByIdUseCase.ExecuteAsync(id, ct);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

   [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken ct)
    {
        var id = await _createProductUseCase.ExecuteAsync(
            request.Name,
            request.Sku,
            request.Price,
            ct);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken ct)
    {
        var updated = await _updateProductUseCase.ExecuteAsync(id, request, ct);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _deleteProductUseCase.ExecuteAsync(id, ct);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

public sealed record CreateProductRequest(
    string Name,
    string Sku,
    decimal Price
);
