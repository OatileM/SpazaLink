using Amazon.DynamoDBv2;
using SpazaLink.Services.Inventory.Repositories;
using SpazaLink.Shared.Models;

// Create web application builder for Inventory service
var builder = WebApplication.CreateBuilder(args);

// Configure services for dependency injection
builder.Services.AddOpenApi(); // Add OpenAPI/Swagger support for API documentation
builder.Services.AddAWSService<IAmazonDynamoDB>(); // Register AWS DynamoDB client for af-south-1 region
builder.Services.AddScoped<IProductRepository, DynamoDbProductRepository>(); // Register product repository with DI

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Enable OpenAPI/Swagger UI in development environment
}

app.UseHttpsRedirection(); // Enforce HTTPS for all requests

// API Endpoints for Product Management

/// <summary>
/// Search products with optional filters
/// Supports filtering by category, area, and price range
/// </summary>
app.MapGet("/products", async (IProductRepository repository, string? category = null, string? area = null, decimal? minPrice = null, decimal? maxPrice = null) =>
{
    // Search products using repository with applied filters
    var products = await repository.SearchProductsAsync(category, area, minPrice, maxPrice);
    return Results.Ok(products);
})
.WithName("SearchProducts")
.WithSummary("Search products with filters")
.WithDescription("Retrieve products filtered by category, area, and/or price range. All filters are optional.");

/// <summary>
/// Retrieve a specific product by its unique identifier
/// </summary>
app.MapGet("/products/{id:guid}", async (Guid id, IProductRepository repository) =>
{
    // Fetch product from repository by ID
    var product = await repository.GetProductByIdAsync(id);
    
    // Return product if found, otherwise return 404 Not Found
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
.WithSummary("Get product details")
.WithDescription("Retrieve detailed information for a specific product using its unique ID");

/// <summary>
/// Calculate total price for a product based on quantity
/// Useful for order calculations and pricing estimates
/// </summary>
app.MapGet("/products/{id:guid}/price", async (Guid id, int quantity, IProductRepository repository) =>
{
    // Fetch product to get base price
    var product = await repository.GetProductByIdAsync(id);
    if (product is null) return Results.NotFound();

    // Calculate total price based on quantity
    var totalPrice = product.BasePrice * quantity;
    
    // Return pricing information as anonymous object
    var result = new { 
        ProductId = id, 
        Quantity = quantity, 
        UnitPrice = product.BasePrice, 
        TotalPrice = totalPrice 
    };
    
    return Results.Ok(result);
})
.WithName("CalculateProductPrice")
.WithSummary("Calculate price based on quantity")
.WithDescription("Calculate total price for a product given a specific quantity. Returns unit price and total price.");

// Start the application
app.Run();