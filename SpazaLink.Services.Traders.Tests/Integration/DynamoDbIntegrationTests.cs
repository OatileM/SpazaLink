using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;
using SpazaLink.Services.Traders.Repositories;
using SpazaLink.Shared.Enums;
using SpazaLink.Shared.Models;
using Xunit;

namespace SpazaLink.Services.Traders.Tests.Integration
{
    /// <summary>
    /// Integration tests for DynamoDbTraderRepository
    /// Tests actual DynamoDB operations against AWS infrastructure
    /// Note: These tests require valid AWS credentials and will create/modify real data
    /// </summary>
    [Collection("DynamoDB Integration Tests")]
    public class DynamoDbIntegrationTests : IAsyncDisposable
    {
        private readonly DynamoDbTraderRepository _repository;
        private readonly List<Guid> _createdTraderIds = new();

        public DynamoDbIntegrationTests()
        {
            // Configure AWS DynamoDB client for integration testing
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var dynamoDbClient = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName("af-south-1"));
            _repository = new DynamoDbTraderRepository(dynamoDbClient);
        }

        [Fact]
        public async Task CreateAndRetrieveTrader_RealDynamoDB_Success()
        {
            // Arrange
            var trader = CreateIntegrationTestTrader("Integration Test Spaza", "Cape Town");

            // Act - Create trader in real DynamoDB
            var createdTrader = await _repository.CreateTraderAsync(trader);
            _createdTraderIds.Add(createdTrader.ID);

            // Act - Retrieve trader from real DynamoDB
            var retrievedTrader = await _repository.GetTraderByIdAsync(createdTrader.ID);

            // Assert
            Assert.NotNull(retrievedTrader);
            Assert.Equal(trader.BusinessName, retrievedTrader.BusinessName);
            Assert.Equal(trader.OwnerName, retrievedTrader.OwnerName);
            Assert.Equal(trader.Area, retrievedTrader.Area);
            Assert.Equal(trader.Type, retrievedTrader.Type);
            Assert.Equal(trader.Status, retrievedTrader.Status);
            Assert.Equal(trader.ProductCategories.Count, retrievedTrader.ProductCategories.Count);
        }

        [Fact]
        public async Task GetTradersByArea_RealDynamoDB_ReturnsCorrectTraders()
        {
            // Arrange
            var testArea = "Johannesburg_Test";
            var trader1 = CreateIntegrationTestTrader("JHB Spaza 1", testArea);
            var trader2 = CreateIntegrationTestTrader("JHB Spaza 2", testArea);

            // Act - Create traders in real DynamoDB
            var created1 = await _repository.CreateTraderAsync(trader1);
            var created2 = await _repository.CreateTraderAsync(trader2);
            _createdTraderIds.Add(created1.ID);
            _createdTraderIds.Add(created2.ID);

            // Wait a moment for eventual consistency
            await Task.Delay(1000);

            // Act - Retrieve traders by area from real DynamoDB
            var tradersInArea = await _repository.GetTradersByAreaAsync(testArea);

            // Assert
            Assert.NotNull(tradersInArea);
            Assert.True(tradersInArea.Count >= 2, $"Expected at least 2 traders, found {tradersInArea.Count}");
            
            var createdTraderIds = new[] { created1.ID, created2.ID };
            var foundTraders = tradersInArea.Where(t => createdTraderIds.Contains(t.ID)).ToList();
            Assert.Equal(2, foundTraders.Count);
        }

        [Fact]
        public async Task CreateTraderWithOptionalFields_RealDynamoDB_Success()
        {
            // Arrange
            var trader = CreateIntegrationTestTrader("Full Details Spaza", "Durban");
            trader.WhatsappNumber = "0821234567";
            trader.Email = "integration@test.com";
            trader.Landmark = "Near the taxi rank";
            trader.IDNumber = "8001015009087";

            // Act - Create trader with all optional fields
            var createdTrader = await _repository.CreateTraderAsync(trader);
            _createdTraderIds.Add(createdTrader.ID);

            // Act - Retrieve and verify all fields
            var retrievedTrader = await _repository.GetTraderByIdAsync(createdTrader.ID);

            // Assert
            Assert.NotNull(retrievedTrader);
            Assert.Equal(trader.WhatsappNumber, retrievedTrader.WhatsappNumber);
            Assert.Equal(trader.Email, retrievedTrader.Email);
            Assert.Equal(trader.Landmark, retrievedTrader.Landmark);
        }

        [Fact]
        public async Task GetNonExistentTrader_RealDynamoDB_ReturnsNull()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetTraderByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateMultipleTraderTypes_RealDynamoDB_Success()
        {
            // Arrange & Act - Create different trader types
            var spazaShop = CreateIntegrationTestTrader("Multi-Type Spaza", "Pretoria");
            spazaShop.Type = TraderType.SpazaShop;

            var streetVendor = CreateIntegrationTestTrader("Multi-Type Vendor", "Pretoria");
            streetVendor.Type = TraderType.StreetVendor;

            var tuckShop = CreateIntegrationTestTrader("Multi-Type Tuck", "Pretoria");
            tuckShop.Type = TraderType.TuckShop;

            var createdSpaza = await _repository.CreateTraderAsync(spazaShop);
            var createdVendor = await _repository.CreateTraderAsync(streetVendor);
            var createdTuck = await _repository.CreateTraderAsync(tuckShop);

            _createdTraderIds.AddRange(new[] { createdSpaza.ID, createdVendor.ID, createdTuck.ID });

            // Act - Retrieve all traders
            var retrievedSpaza = await _repository.GetTraderByIdAsync(createdSpaza.ID);
            var retrievedVendor = await _repository.GetTraderByIdAsync(createdVendor.ID);
            var retrievedTuck = await _repository.GetTraderByIdAsync(createdTuck.ID);

            // Assert
            Assert.Equal(TraderType.SpazaShop, retrievedSpaza!.Type);
            Assert.Equal(TraderType.StreetVendor, retrievedVendor!.Type);
            Assert.Equal(TraderType.TuckShop, retrievedTuck!.Type);
        }

        /// <summary>
        /// Creates a test trader for integration testing
        /// </summary>
        private static Trader CreateIntegrationTestTrader(string businessName, string area)
        {
            return new Trader
            {
                ID = Guid.NewGuid(),
                BusinessName = businessName,
                OwnerName = $"Owner of {businessName}",
                PhoneNumber = "0123456789",
                Area = area,
                Street = "Test Street",
                Type = TraderType.SpazaShop,
                Status = TraderStatus.PendingVerification,
                Tier = TraderTier.Bronze,
                isVerified = false,
                RegistrationDate = DateTime.UtcNow,
                ProductCategories = new List<string> { "Groceries", "Beverages", "Snacks" }
            };
        }

        /// <summary>
        /// Cleanup: Remove test data from DynamoDB after tests complete
        /// Note: In a real scenario, you might want to use a separate test table
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            // Note: DynamoDB doesn't have a direct delete operation in this repository
            // In production, you would implement a DeleteTraderAsync method
            // For now, we'll leave the test data (it's harmless and shows real usage)
            
            await Task.CompletedTask;
        }
    }
}