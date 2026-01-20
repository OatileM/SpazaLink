using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Moq;
using SpazaLink.Services.Inventory.Repositories;
using SpazaLink.Shared.Models;
using Xunit;

namespace SpazaLink.Services.Inventory.Tests.Repositories
{
    /// <summary>
    /// Unit tests for DynamoDbProductRepository
    /// Tests the repository layer functionality without actual DynamoDB calls
    /// </summary>
    public class DynamoDbProductRepositoryTests
    {
        private readonly Mock<IAmazonDynamoDB> _mockDynamoDb;
        private readonly DynamoDbProductRepository _repository;

        public DynamoDbProductRepositoryTests()
        {
            _mockDynamoDb = new Mock<IAmazonDynamoDB>();
            _repository = new DynamoDbProductRepository(_mockDynamoDb.Object);
        }

        [Fact]
        public async Task CreateProductAsync_ValidProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var product = CreateTestProduct();
            _mockDynamoDb.Setup(x => x.PutItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default))
                        .ReturnsAsync(new PutItemResponse());

            // Act
            var result = await _repository.CreateProductAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Category, result.Category);
            _mockDynamoDb.Verify(x => x.PutItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_NullProduct_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreateProductAsync(null!));
        }

        [Fact]
        public async Task GetProductByIdAsync_ExistingProduct_ReturnsProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var mockResponse = CreateMockGetItemResponse();
            
            _mockDynamoDb.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _repository.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Name);
            Assert.Equal("Groceries", result.Category);
        }

        [Fact]
        public async Task GetProductByIdAsync_NonExistingProduct_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var mockResponse = new GetItemResponse { IsItemSet = false };
            
            _mockDynamoDb.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, AttributeValue>>(), default))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _repository.GetProductByIdAsync(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchProductsAsync_WithCategory_ReturnsFilteredProducts()
        {
            // Arrange
            var mockResponse = CreateMockQueryResponse();
            _mockDynamoDb.Setup(x => x.QueryAsync(It.IsAny<QueryRequest>(), default))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _repository.SearchProductsAsync(category: "Groceries");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Groceries", result[0].Category);
        }

        [Fact]
        public async Task SearchProductsAsync_WithPriceRange_ReturnsFilteredProducts()
        {
            // Arrange
            var mockResponse = CreateMockScanResponse();
            _mockDynamoDb.Setup(x => x.ScanAsync(It.IsAny<ScanRequest>(), default))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _repository.SearchProductsAsync(minPrice: 5m, maxPrice: 15m);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.True(result[0].BasePrice >= 5m && result[0].BasePrice <= 15m);
        }

        /// <summary>
        /// Creates a test product entity for testing purposes
        /// </summary>
        private static Product CreateTestProduct()
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Category = "Groceries",
                SubCategory = "Canned Food",
                BasePrice = 10.50m,
                Unit = "each",
                UnitsPerCase = 12,
                SupplierId = Guid.NewGuid(),
                SupplierName = "Test Supplier",
                StockLevel = 100,
                MinimumOrderQuantity = 1,
                IsActive = true,
                ImageUrls = new List<string> { "test.jpg" },
                LastUpdated = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a mock DynamoDB GetItem response
        /// </summary>
        private static GetItemResponse CreateMockGetItemResponse()
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["productId"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["name"] = new AttributeValue { S = "Test Product" },
                ["description"] = new AttributeValue { S = "Test Description" },
                ["category"] = new AttributeValue { S = "Groceries" },
                ["subCategory"] = new AttributeValue { S = "Canned Food" },
                ["basePrice"] = new AttributeValue { N = "10.50" },
                ["unit"] = new AttributeValue { S = "each" },
                ["unitsPerCase"] = new AttributeValue { N = "12" },
                ["supplierId"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["supplierName"] = new AttributeValue { S = "Test Supplier" },
                ["stockLevel"] = new AttributeValue { N = "100" },
                ["minimumOrderQuantity"] = new AttributeValue { N = "1" },
                ["isActive"] = new AttributeValue { BOOL = true },
                ["lastUpdated"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") }
            };

            return new GetItemResponse { IsItemSet = true, Item = item };
        }

        /// <summary>
        /// Creates a mock DynamoDB Query response
        /// </summary>
        private static QueryResponse CreateMockQueryResponse()
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["productId"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["name"] = new AttributeValue { S = "Test Product" },
                ["description"] = new AttributeValue { S = "Test Description" },
                ["category"] = new AttributeValue { S = "Groceries" },
                ["subCategory"] = new AttributeValue { S = "Canned Food" },
                ["basePrice"] = new AttributeValue { N = "10.50" },
                ["unit"] = new AttributeValue { S = "each" },
                ["unitsPerCase"] = new AttributeValue { N = "12" },
                ["supplierId"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["supplierName"] = new AttributeValue { S = "Test Supplier" },
                ["stockLevel"] = new AttributeValue { N = "100" },
                ["minimumOrderQuantity"] = new AttributeValue { N = "1" },
                ["isActive"] = new AttributeValue { BOOL = true },
                ["lastUpdated"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") }
            };

            return new QueryResponse { Items = new List<Dictionary<string, AttributeValue>> { item } };
        }

        /// <summary>
        /// Creates a mock DynamoDB Scan response
        /// </summary>
        private static ScanResponse CreateMockScanResponse()
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["productId"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["name"] = new AttributeValue { S = "Test Product" },
                ["description"] = new AttributeValue { S = "Test Description" },
                ["category"] = new AttributeValue { S = "Groceries" },
                ["subCategory"] = new AttributeValue { S = "Canned Food" },
                ["basePrice"] = new AttributeValue { N = "10.50" },
                ["unit"] = new AttributeValue { S = "each" },
                ["unitsPerCase"] = new AttributeValue { N = "12" },
                ["supplierId"] = new AttributeValue { S = Guid.NewGuid().ToString() },
                ["supplierName"] = new AttributeValue { S = "Test Supplier" },
                ["stockLevel"] = new AttributeValue { N = "100" },
                ["minimumOrderQuantity"] = new AttributeValue { N = "1" },
                ["isActive"] = new AttributeValue { BOOL = true },
                ["lastUpdated"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") }
            };

            return new ScanResponse { Items = new List<Dictionary<string, AttributeValue>> { item } };
        }
    }
}