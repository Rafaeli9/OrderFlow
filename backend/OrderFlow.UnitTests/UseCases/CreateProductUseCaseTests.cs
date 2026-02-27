using OrderFlow.Application.UseCases.CreateProduct;
using OrderFlow.UnitTests.Fakes;
using Xunit;

namespace OrderFlow.UnitTests.UseCases;

public sealed class CreateProductUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_Should_Create_Product_When_Sku_Is_Unique()
    {
        // Arrange
        var repo = new FakeProductRepository();
        var useCase = new CreateProductUseCase(repo);

        // Act
        var id = await useCase.ExecuteAsync("Notebook Apple", "notemac", 6500);

        // Assert
        Assert.NotEqual(Guid.Empty, id);

        var all = await repo.ListAsync();
        Assert.Single(all);

        var created = all[0];
        Assert.Equal(id, created.Id);
        Assert.Equal("Notebook Apple", created.Name);
        Assert.Equal("NOTEMAC", created.Sku);
        Assert.Equal(6500, created.Price);
        Assert.True(created.Active);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_When_Sku_Already_Exists()
    {
        // Arrange
        var repo = new FakeProductRepository();
        var useCase = new CreateProductUseCase(repo);

        await useCase.ExecuteAsync("Produto A", "SKU123", 10);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            useCase.ExecuteAsync("Produto B", "sku123", 20));

        Assert.Contains("SKU", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}