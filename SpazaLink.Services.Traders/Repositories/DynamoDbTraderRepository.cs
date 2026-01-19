using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using SpazaLink.Shared.Models;
using SpazaLink.Shared.Enums;

namespace SpazaLink.Services.Traders.Repositories
{
    /// <summary>
    /// DynamoDB implementation of the trader repository
    /// Handles all trader data persistence operations using AWS DynamoDB
    /// </summary>
    public class DynamoDbTraderRepository : ITraderRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly string _tableName = "Traders";

        /// <summary>
        /// Initializes a new instance of the DynamoDbTraderRepository
        /// </summary>
        /// <param name="dynamoDb">The DynamoDB client instance</param>
        /// <exception cref="ArgumentNullException">Thrown when dynamoDb is null</exception>
        public DynamoDbTraderRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
        }

        /// <inheritdoc/>
        public async Task<Trader> CreateTraderAsync(Trader trader)
        {
            if (trader == null)
                throw new ArgumentNullException(nameof(trader));

            // Map trader entity to DynamoDB attribute values
            var item = new Dictionary<string, AttributeValue>
            {
                ["ID"] = new AttributeValue { S = trader.ID.ToString() },
                ["BusinessName"] = new AttributeValue { S = trader.BusinessName },
                ["OwnerName"] = new AttributeValue { S = trader.OwnerName },
                ["PhoneNumber"] = new AttributeValue { S = trader.PhoneNumber },
                ["Area"] = new AttributeValue { S = trader.Area },
                ["Street"] = new AttributeValue { S = trader.Street },
                ["Type"] = new AttributeValue { S = trader.Type.ToString() },
                ["Status"] = new AttributeValue { S = trader.Status.ToString() },
                ["Tier"] = new AttributeValue { S = trader.Tier.ToString() },
                ["IsVerified"] = new AttributeValue { BOOL = trader.isVerified },
                ["RegistrationDate"] = new AttributeValue { S = trader.RegistrationDate.ToString("O") },
                ["ProductCategories"] = new AttributeValue { SS = trader.ProductCategories }
            };

            // Add optional contact information if provided
            if (!string.IsNullOrEmpty(trader.WhatsappNumber))
                item["WhatsappNumber"] = new AttributeValue { S = trader.WhatsappNumber };

            if (!string.IsNullOrEmpty(trader.Email))
                item["Email"] = new AttributeValue { S = trader.Email };

            if (!string.IsNullOrEmpty(trader.Landmark))
                item["Landmark"] = new AttributeValue { S = trader.Landmark };

            if (!string.IsNullOrEmpty(trader.IDNumber))
                item["IDNumber"] = new AttributeValue { S = trader.IDNumber };

            // Store the trader in DynamoDB
            await _dynamoDb.PutItemAsync(_tableName, item);
            return trader;
        }

        /// <inheritdoc/>
        public async Task<Trader?> GetTraderByIdAsync(Guid id)
        {
            // Create the primary key for the query
            var key = new Dictionary<string, AttributeValue>
            {
                ["ID"] = new AttributeValue { S = id.ToString() }
            };

            // Retrieve the item from DynamoDB
            var response = await _dynamoDb.GetItemAsync(_tableName, key);

            // Return null if trader not found
            if (!response.IsItemSet)
                return null;

            // Map DynamoDB item back to trader entity
            return MapToTrader(response.Item);
        }

        /// <inheritdoc/>
        public async Task<List<Trader>> GetTradersByAreaAsync(string area)
        {
            if (string.IsNullOrEmpty(area))
                throw new ArgumentException("Area cannot be null or empty", nameof(area));

            // Create scan request to find traders by area
            // Note: In production, consider using GSI for better performance
            var request = new ScanRequest
            {
                TableName = _tableName,
                FilterExpression = "Area = :area",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [":area"] = new AttributeValue { S = area }
                }
            };

            var response = await _dynamoDb.ScanAsync(request);
            return response.Items.Select(MapToTrader).ToList();
        }

        /// <summary>
        /// Maps DynamoDB attribute values to a Trader entity
        /// </summary>
        /// <param name="item">Dictionary of DynamoDB attribute values</param>
        /// <returns>Mapped Trader entity</returns>
        private Trader MapToTrader(Dictionary<string, AttributeValue> item)
        {
            return new Trader
            {
                ID = Guid.Parse(item["ID"].S),
                BusinessName = item["BusinessName"].S,
                OwnerName = item["OwnerName"].S,
                PhoneNumber = item["PhoneNumber"].S,
                Area = item["Area"].S,
                Street = item["Street"].S,
                Type = Enum.Parse<TraderType>(item["Type"].S),
                Status = Enum.Parse<TraderStatus>(item["Status"].S),
                Tier = Enum.Parse<TraderTier>(item["Tier"].S),
                isVerified = item["IsVerified"].BOOL ?? false,
                RegistrationDate = DateTime.Parse(item["RegistrationDate"].S),
                ProductCategories = item["ProductCategories"].SS.ToList(),
                WhatsappNumber = item.ContainsKey("WhatsappNumber") ? item["WhatsappNumber"].S : null,
                Email = item.ContainsKey("Email") ? item["Email"].S : null,
                Landmark = item.ContainsKey("Landmark") ? item["Landmark"].S : string.Empty,
                IDNumber = item.ContainsKey("IDNumber") ? item["IDNumber"].S : null
            };
        }
    }
}
