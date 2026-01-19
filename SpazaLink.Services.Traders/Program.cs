using SpazaLink.Shared.Models;
using SpazaLink.Shared.DTOs;
using SpazaLink.Shared.Enums;
using SpazaLink.Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var trader = new Trader
{
    BusinessName = "Mama's Spaza",
    OwnerName = "Mama Jane",
    PhoneNumber = "0123456789",
    Area = "Downtown",
    Street = "Main St",
    Type = TraderType.SpazaShop,
    ProductCategories = new List<string> { "Groceries", "Snacks" },
    Status = TraderStatus.Active,
    Tier = TraderTier.Bronze,
    isVerified = true,
    RegistrationDate = DateTime.UtcNow
};

var request = new CreateTraderRequest
{
    BusinessName = "Papa's TuckShop",
    OwnerName = "Papa John",
    PhoneNumber = "0987654321",
    Area = "Uptown",
    Street = "2nd Ave",
    Type = TraderType.TuckShop,
    ProductCategories = new List<string> { "Beverages", "Fast Food" },
    WhatsAppNumber = "0987654321",
    Email = "osmdlela@gmail.com"
};


app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


app.MapPost("/createtrader", (CreateTraderRequest req) =>
{
    // Here you would normally add logic to save the trader to a database
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
        Status = TraderStatus.PendingVerification,
        Tier = TraderTier.Bronze,
        isVerified = false,
        RegistrationDate = DateTime.UtcNow
    };
    // Simulate saving to database and returning the created trader
    return Results.Created($"/traders/{newTrader.ID}", newTrader);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
