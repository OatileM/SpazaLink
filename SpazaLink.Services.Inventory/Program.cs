using Amazon.DynamoDBv2;
using SpazaLink.Services.Inventory.Repositories;
using SpazaLink.Shared.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAWSService<IAmazonDynamoDB>(); // Register AWS DynamoDB client
builder.Services.AddScoped<IProductRepository, DynamoDbProductRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/products", async (IProductRepository repository, string? category = null, string? area = null, decimal? minPrice = null, decimal? maxPrice = null) =>
{
    var products = await repository.SearchProductsAsync(category, area, minPrice, maxPrice);
    return Results.Ok(products);
})
.WithName("SearchProducts")
.WithSummary("Search products with filters");

// GET /products/{id} - Get product details
app.MapGet("/products/{id:guid}", async (Guid id, IProductRepository repository) =>
{
    var product = await repository.GetProductByIdAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
.WithSummary("Get product details");

// GET /products/{id}/price - Calculate price based on quantity
app.MapGet("/products/{id:guid}/price", async (Guid id, int quantity, IProductRepository repository) =>
{
    var product = await repository.GetProductByIdAsync(id);
    if (product is null) return Results.NotFound();

    var totalPrice = product.BasePrice * quantity;
    var result = new { ProductId = id, Quantity = quantity, UnitPrice = product.BasePrice, TotalPrice = totalPrice };
    return Results.Ok(result);
})
.WithName("CalculateProductPrice")
.WithSummary("Calculate price based on quantity");

app.Run();