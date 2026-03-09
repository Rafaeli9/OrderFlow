using Microsoft.EntityFrameworkCore;
using OrderFlow.Infrastructure.Persistence;
using OrderFlow.Application.Abstractions;
using OrderFlow.Infrastructure.Repositories;
using OrderFlow.Application.UseCases.CreateProduct;
using OrderFlow.Application.UseCases.ListProducts;
using OrderFlow.Application.UseCases.GetProductById;
using OrderFlow.Application.UseCases.UpdateProduct;
using OrderFlow.Application.UseCases.DeleteProduct;
using OrderFlow.Application.UseCases.ActivateProduct;
using OrderFlow.Api.Middlewares;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderFlowDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres");
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<ListProductsUseCase>();
builder.Services.AddScoped<GetProductByIdUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<ActivateProductUseCase>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.MapControllers();

app.Run();
