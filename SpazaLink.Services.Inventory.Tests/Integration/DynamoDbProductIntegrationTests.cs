using Amazon.DynamoDBv2;
using SpazaLink.Services.Inventory.Repositories;
using SpazaLink.Shared.Models;
using Xunit;

namespace SpazaLink.Services.Inventory.Tests.Integration
{
    /// <summary>
    /// Integration tests for DynamoDbProductRepository
    /// Tests actual DynamoDB operations against AWS infrastructure
    /// Note: These tests require valid AWS credentials and will create/modify real data
    /// </summary>
    [Collection("DynamoDB Integration Tests")]
    public class DynamoDbProductIntegrationTests : IAsyncDisposable
    {
        private readonly DynamoDbProductRepository _repository;
        private readonly List<Guid> _createdProductIds = new();

        public DynamoDbProductIntegrationTests()
        {
            var dynamoDbClient = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName("af-south-1"));
            _repository = new DynamoDbProductRepository(dynamoDbClient);
        }

        [Fact]
        public async Task CreateAndRetrieveProduct_RealDynamoDB_Success()
        {
            // Arrange
            var product = CreateIntegrationTestProduct("Integration Test Product", "Groceries");

            // Act - Create product in real DynamoDB
            var createdProduct = await _repository.CreateProductAsync(product);
            _createdProductIds.Add(createdProduct.Id);

            // Act - Retrieve product from real DynamoDB
            var retrievedProduct = await _repository.GetProductByIdAsync(createdProduct.Id);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.Equal(product.Name, retrievedProduct.Name);
            Assert.Equal(product.Category, retrievedProduct.Category);
            Assert.Equal(product.BasePrice, retrievedProduct.BasePrice);
            Assert.Equal(product.StockLevel, retrievedProduct.StockLevel);
        }

        [Fact]
        public async Task SearchProductsByCategory_RealDynamoDB_ReturnsCorrectProducts()
        {
            // Arrange
            var testCategory = "Beverages_Test";
            var product1 = CreateIntegrationTestProduct("Beverages Product 1", testCategory);
            var product2 = CreateIntegrationTestProduct("Beverages Product 2", testCategory);

            // Act - Create products in real DynamoDB
            var created1 = await _repository.CreateProductAsync(product1);
            var created2 = await _repository.CreateProductAsync(product2);
            _createdProductIds.AddRange(new[] { created1.Id, created2.Id });

            // Wait for eventual consistency
            await Task.Delay(1000);

            // Act - Search products by category from real DynamoDB
            var products = await _repository.SearchProductsAsync(category: testCategory);

            // Assert
            Assert.NotNull(products);
            Assert.True(products.Count >= 2, $"Expected at least 2 products, found {products.Count}");
            
            var createdProductIds = new[] { created1.Id, created2.Id };
            var foundProducts = products.Where(p => createdProductIds.Contains(p.Id)).ToList();
            Assert.Equal(2, foundProducts.Count);
            Assert.All(foundProducts, p => Assert.Equal(testCategory, p.Category));
        }

        [Fact]
        public async Task SearchProductsWithPriceFilter_RealDynamoDB_ReturnsFilteredProducts()
        {
            // Arrange
            var expensiveProduct = CreateIntegrationTestProduct("Expensive Product", "Electronics");
            expensiveProduct.BasePrice = 100.00m;

            var cheapProduct = CreateIntegrationTestProduct("Cheap Product", "Electronics");
            cheapProduct.BasePrice = 5.00m;

            // Act - Create products
            var created1 = await _repository.CreateProductAsync(expensiveProduct);
            var created2 = await _repository.CreateProductAsync(cheapProduct);
            _createdProductIds.AddRange(new[] { created1.Id, created2.Id });

            await Task.Delay(1000);

            // Act - Search with price filter
            var expensiveProducts = await _repository.SearchProductsAsync(minPrice: 50m);
            var cheapProducts = await _repository.SearchProductsAsync(maxPrice: 10m);

            // Assert
            var foundExpensive = expensiveProducts.FirstOrDefault(p => p.Id == created1.Id);
            var foundCheap = cheapProducts.FirstOrDefault(p => p.Id == created2.Id);

            Assert.NotNull(foundExpensive);
            Assert.True(foundExpensive.BasePrice >= 50m);
            
            Assert.NotNull(foundCheap);
            Assert.True(foundCheap.BasePrice <= 10m);
        }

        [Fact]
        public async Task GetNonExistentProduct_RealDynamoDB_ReturnsNull()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetProductByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProductWithAllFields_RealDynamoDB_Success()
        {
            // Arrange
            var product = CreateIntegrationTestProduct("Full Details Product", "Snacks");
            product.ImageUrls = new List<string> { "image1.jpg", "image2.jpg" };
            product.SubCategory = "Chips";
            product.UnitsPerCase = 24;

            // Act - Create product with all fields
            var createdProduct = await _repository.CreateProductAsync(product);
            _createdProductIds.Add(createdProduct.Id);

            // Act - Retrieve and verify all fields
            var retrievedProduct = await _repository.GetProductByIdAsync(createdProduct.Id);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.Equal(product.SubCategory, retrievedProduct.SubCategory);
            Assert.Equal(product.UnitsPerCase, retrievedProduct.UnitsPerCase);
            Assert.Equal(product.ImageUrls.Count, retrievedProduct.ImageUrls.Count);
            Assert.Equal(product.MinimumOrderQuantity, retrievedProduct.MinimumOrderQuantity);
        }

        /// <summary>
        /// Creates a test product for integration testing
        /// </summary>
        private static Product CreateIntegrationTestProduct(string name, string category)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = $"Integration test for {name}",
                Category = category,
                SubCategory = "Test SubCategory",
                BasePrice = 15.99m,
                Unit = "each",
                UnitsPerCase = 6,
                SupplierId = Guid.NewGuid(),
                SupplierName = "Integration Test Supplier",
                StockLevel = 50,
                MinimumOrderQuantity = 1,
                IsActive = true,
                ImageUrls = new List<string> { "test-image.jpg" },
                LastUpdated = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Cleanup: Remove test data from DynamoDB after tests complete
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            // Note: DynamoDB doesn't have a direct delete operation in this repository
            // In production, you would implement a DeleteProductAsync method
            await Task.CompletedTask;
        }
    }
}