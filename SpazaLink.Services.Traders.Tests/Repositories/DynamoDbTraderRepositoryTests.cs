using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Moq;
using SpazaLink.Services.Traders.Repositories;
using SpazaLink.Shared.Enums;
using SpazaLink.Shared.Models;
using Xunit;

namespace SpazaLink.Services.Traders.Tests.Repositories
{
    /// <summary>
    /// Unit tests for DynamoDbTraderRepository
    /// Tests the repository layer functionality without actual DynamoDB calls
    /// </summary>
    public class DynamoDbTraderRepositoryTests
    {
        private readonly Mock<IAmazonDynamoDB> _mockDynamoDb;
        private readonly DynamoDbTraderRepository _repository;

        public DynamoDbTraderRepositoryTests()
        {
            _mockDynamoDb = new Mock<IAmazonDynamoDB>();
            _repository = new DynamoDbTraderRepository(_mockDynamoDb.Object);
        }

        [Fact]
        public async Task CreateTraderAsync_ValidTrader_ReturnsCreatedTrader()
        {
            // Arrange
            var trader = CreateTestTrader();
            _mockDynamoDb.Setup(x => x.PutItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default))
                        .ReturnsAsync(new PutItemResponse());

            // Act
            var result = await _repository.CreateTraderAsync(trader);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(trader.BusinessName, result.BusinessName);
            Assert.Equal(trader.OwnerName, result.OwnerName);
            _mockDynamoDb.Verify(x => x.PutItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default), Times.Once);
        }

        [Fact]
        public async Task CreateTraderAsync_NullTrader_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreateTraderAsync(null!));
        }

        [Fact]
        public async Task GetTraderByIdAsync_ExistingTrader_ReturnsTrader()
        {
            // Arrange
            var traderId = Guid.NewGuid();
            var mockResponse = CreateMockGetItemResponse();
            
            _mockDynamoDb.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _repository.GetTraderByIdAsync(traderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Spaza", result.BusinessName);
            Assert.Equal("John Doe", result.OwnerName);
        }

        [Fact]
        public async Task GetTraderByIdAsync_NonExistingTrader_ReturnsNull()
        {
            // Arrange
            var traderId = Guid.NewGuid();
            var mockResponse = new GetItemResponse { IsItemSet = false };
            
            _mockDynamoDb.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _repository.GetTraderByIdAsync(traderId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTradersByAreaAsync_ValidArea_ReturnsTraders()
        {
            // Arrange
            var area = "Downtown";
            var mockResponse = CreateMockScanResponse();
            
            _mockDynamoDb.Setup(x => x.ScanAsync(It.IsAny<ScanRequest>(), default))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _repository.GetTradersByAreaAsync(area);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test Spaza", result[0].BusinessName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetTradersByAreaAsync_InvalidArea_ThrowsArgumentException(string area)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _repository.GetTradersByAreaAsync(area));
        }

        /// <summary>
        /// Creates a test trader entity for testing purposes
        /// </summary>
        private static Trader CreateTestTrader()
        {
            return new Trader
            {
                ID = Guid.NewGuid(),
                BusinessName = "Test Spaza",
                OwnerName = "John Doe",
                PhoneNumber = "0123456789",
                Area = "Downtown",
                Street = "Main Street",
                Type = TraderType.SpazaShop,
                Status = TraderStatus.PendingVerification,
                Tier = TraderTier.Bronze,
                isVerified = false,
                RegistrationDate = DateTime.UtcNow,
                ProductCategories = new List<string> { "Groceries", "Snacks" },
                WhatsappNumber = "0123456789",
                Email = "test@example.com"
            };
        }

        /// <summary>
        /// Creates a mock DynamoDB GetItem response
        /// </summary>
        private static GetItemResponse CreateMockGetItemResponse()
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["ID"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["BusinessName"] = new AttributeValue { S = "Test Spaza" },
                ["OwnerName"] = new AttributeValue { S = "John Doe" },
                ["PhoneNumber"] = new AttributeValue { S = "0123456789" },
                ["Area"] = new AttributeValue { S = "Downtown" },
                ["Street"] = new AttributeValue { S = "Main Street" },
                ["Type"] = new AttributeValue { S = "SpazaShop" },
                ["Status"] = new AttributeValue { S = "PendingVerification" },
                ["Tier"] = new AttributeValue { S = "Bronze" },
                ["IsVerified"] = new AttributeValue { BOOL = false },
                ["RegistrationDate"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") },
                ["ProductCategories"] = new AttributeValue { SS = new List<string> { "Groceries", "Snacks" } }
            };

            return new GetItemResponse
            {
                IsItemSet = true,
                Item = item
            };
        }

        /// <summary>
        /// Creates a mock DynamoDB Scan response
        /// </summary>
        private static ScanResponse CreateMockScanResponse()
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["ID"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["BusinessName"] = new AttributeValue { S = "Test Spaza" },
                ["OwnerName"] = new AttributeValue { S = "John Doe" },
                ["PhoneNumber"] = new AttributeValue { S = "0123456789" },
                ["Area"] = new AttributeValue { S = "Downtown" },
                ["Street"] = new AttributeValue { S = "Main Street" },
                ["Type"] = new AttributeValue { S = "SpazaShop" },
                ["Status"] = new AttributeValue { S = "PendingVerification" },
                ["Tier"] = new AttributeValue { S = "Bronze" },
                ["IsVerified"] = new AttributeValue { BOOL = false },
                ["RegistrationDate"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") },
                ["ProductCategories"] = new AttributeValue { SS = new List<string> { "Groceries", "Snacks" } }
            };

            return new ScanResponse
            {
                Items = new List<Dictionary<string, AttributeValue>> { item }
            };
        }
    }
}