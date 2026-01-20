using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using SpazaLink.Shared.Models;

namespace SpazaLink.Services.Inventory.Repositories
{
    public class DynamoDbProductRepository: IProductRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly string _tableName = "Products";

        public DynamoDbProductRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
        }

        public async Task<List<Product>> SearchProductsAsync(string? category = null, string? area = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            if (!string.IsNullOrEmpty(category))
            {
                return await SearchByCategoryAsync(category, minPrice, maxPrice);
            }

            return await ScanAllProductsAsync(minPrice, maxPrice);
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                ["productId"] = new AttributeValue { S = id.ToString() }
            };

            var response = await _dynamoDb.GetItemAsync(_tableName, key);
            return response.IsItemSet ? MapToProduct(response.Item) : null;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var item = new Dictionary<string, AttributeValue>
            {
                ["productId"] = new AttributeValue { S = product.Id.ToString() },
                ["name"] = new AttributeValue { S = product.Name },
                ["description"] = new AttributeValue { S = product.Description },
                ["category"] = new AttributeValue { S = product.Category },
                ["subCategory"] = new AttributeValue { S = product.SubCategory },
                ["basePrice"] = new AttributeValue { N = product.BasePrice.ToString(System.Globalization.CultureInfo.InvariantCulture) },
                ["unit"] = new AttributeValue { S = product.Unit },
                ["unitsPerCase"] = new AttributeValue { N = product.UnitsPerCase.ToString() },
                ["supplierId"] = new AttributeValue { S = product.SupplierId.ToString() },
                ["supplierName"] = new AttributeValue { S = product.SupplierName },
                ["stockLevel"] = new AttributeValue { N = product.StockLevel.ToString() },
                ["minimumOrderQuantity"] = new AttributeValue { N = product.MinimumOrderQuantity.ToString() },
                ["isActive"] = new AttributeValue { BOOL = product.IsActive },
                ["lastUpdated"] = new AttributeValue { S = product.LastUpdated.ToString("O") }
            };

            if (product.ImageUrls.Any())
                item["imageUrls"] = new AttributeValue { SS = product.ImageUrls };

            await _dynamoDb.PutItemAsync(_tableName, item);
            return product;
        }

        private async Task<List<Product>> SearchByCategoryAsync(string category, decimal? minPrice, decimal? maxPrice)
        {
            var request = new QueryRequest
            {
                TableName = _tableName,
                IndexName = "CategoryIndex",
                KeyConditionExpression = "category = :category",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [":category"] = new AttributeValue { S = category }
                }
            };

            var response = await _dynamoDb.QueryAsync(request);
            var products = response.Items.Select(MapToProduct).ToList();

            return FilterByPrice(products, minPrice, maxPrice);
        }

        private async Task<List<Product>> ScanAllProductsAsync(decimal? minPrice, decimal? maxPrice)
        {
            var request = new ScanRequest { TableName = _tableName };
            var response = await _dynamoDb.ScanAsync(request);
            var products = response.Items.Select(MapToProduct).ToList();

            return FilterByPrice(products, minPrice, maxPrice);
        }

        private static List<Product> FilterByPrice(List<Product> products, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue)
                products = products.Where(p => p.BasePrice >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                products = products.Where(p => p.BasePrice <= maxPrice.Value).ToList();

            return products;
        }

        private static Product MapToProduct(Dictionary<string, AttributeValue> item)
        {
            return new Product
            {
                Id = Guid.Parse(item["productId"].S),
                Name = item["name"].S,
                Description = item["description"].S,
                Category = item["category"].S,
                SubCategory = item["subCategory"].S,
                BasePrice = decimal.Parse(item["basePrice"].N, System.Globalization.CultureInfo.InvariantCulture),
                Unit = item["unit"].S,
                UnitsPerCase = int.Parse(item["unitsPerCase"].N),
                SupplierId = Guid.Parse(item["supplierId"].S),
                SupplierName = item["supplierName"].S,
                StockLevel = int.Parse(item["stockLevel"].N),
                MinimumOrderQuantity = int.Parse(item["minimumOrderQuantity"].N),
                IsActive = item["isActive"].BOOL ?? true,
                LastUpdated = DateTime.Parse(item["lastUpdated"].S),
                ImageUrls = item.ContainsKey("imageUrls") ? item["imageUrls"].SS.ToList() : new List<string>()
            };
        }
    }
}
