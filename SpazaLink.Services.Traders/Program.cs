using Amazon.DynamoDBv2;
using SpazaLink.Shared.Constants;
using SpazaLink.Shared.DTOs;
using SpazaLink.Shared.Enums;
using SpazaLink.Shared.Models;
using SpazaLink.Services.Traders.Repositories;

// Create web application builder
var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddOpenApi(); // Add OpenAPI/Swagger support
builder.Services.AddAWSService<IAmazonDynamoDB>(); // Register AWS DynamoDB client
builder.Services.AddScoped<ITraderRepository, DynamoDbTraderRepository>(); // Register trader repository

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Enable OpenAPI in development
}

app.UseHttpsRedirection(); // Enforce HTTPS

// API Endpoints

/// <summary>
/// Creates a new trader registration
/// </summary>
app.MapPost("/createtrader", async (CreateTraderRequest req, ITraderRepository repository) =>
{
    // Map request DTO to trader entity with default values
    var newTrader = new Trader
    {
        BusinessName = req.BusinessName,
        OwnerName = req.OwnerName,
        PhoneNumber = req.PhoneNumber,
        Area = req.Area,
        Street = req.Street,
        Type = req.Type,
        ProductCategories = req.ProductCategories,
        WhatsappNumber = req.WhatsAppNumber,
        Email = req.Email,
        Status = TraderStatus.PendingVerification, // New traders start as pending
        Tier = TraderTier.Bronze, // All new traders start at Bronze tier
        isVerified = false, // Verification required
        RegistrationDate = DateTime.UtcNow
    };

    // Save trader to database
    var createdTrader = await repository.CreateTraderAsync(newTrader);
    return Results.Created($"/traders/{createdTrader.ID}", createdTrader);
})
.WithName("CreateTrader")
.WithSummary("Register a new trader")
.WithDescription("Creates a new trader registration with pending verification status");

/// <summary>
/// Retrieves a trader by their unique ID
/// </summary>
app.MapGet("/traders/{id:guid}", async (Guid id, ITraderRepository repository) =>
{
    var trader = await repository.GetTraderByIdAsync(id);
    return trader is not null ? Results.Ok(trader) : Results.NotFound();
})
.WithName("GetTraderById")
.WithSummary("Get trader by ID")
.WithDescription("Retrieves a specific trader using their unique identifier");

/// <summary>
/// Retrieves all traders in a specific area
/// </summary>
app.MapGet("/traders/area/{area}", async (string area, ITraderRepository repository) =>
{
    var traders = await repository.GetTradersByAreaAsync(area);
    return Results.Ok(traders);
})
.WithName("GetTradersByArea")
.WithSummary("Get traders by area")
.WithDescription("Retrieves all traders operating in a specific geographical area");

// Start the application
app.Run();
