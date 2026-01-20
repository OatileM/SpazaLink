# SpazaLink

A digital platform connecting local spaza shops, tuck shops, and street vendors with their communities.

## Overview

SpazaLink is a microservices-based solution designed to digitize and connect small-scale traders (spaza shops, tuck shops, street vendors) in South African communities. The platform enables trader registration, verification, and management through a RESTful API with AWS DynamoDB persistence.

## Architecture

- **SpazaLink.Services.Traders**: Web API service for trader management
- **SpazaLink.Services.Inventory**: Product catalog and supplier management API
- **SpazaLink.Shared**: Common models, DTOs, enums, and utilities
- **SpazaLink.Services.Traders.Tests**: Unit tests for the trader service
- **SpazaLink.Services.Inventory.Tests**: Unit and integration tests for inventory service

## Features

- Trader registration and profile management
- Multi-tier trader classification (Bronze, Silver, Gold, Platinum)
- Support for various trader types (Spaza Shop, Street Vendor, Tuck Shop, Home Business)
- Product catalog management with supplier integration
- Supplier network with delivery area mapping
- Verification system with KYC capabilities
- Multiple payment method support
- Multi-channel notifications (SMS, WhatsApp, Email)
- AWS DynamoDB integration for scalable data storage
- Comprehensive unit testing with xUnit and Moq

## Trader Types

- **Spaza Shop**: Small convenience stores
- **Street Vendor**: Mobile/market stall traders
- **Tuck Shop**: School/office-based vendors
- **Home Business**: Home-operated businesses

## API Endpoints

### Traders Service

- `POST /createtrader` - Register a new trader
- `GET /traders/{id}` - Get trader by ID
- `GET /traders/area/{area}` - Get traders by area

### Inventory Service

- `GET /products` - Search products with filters (category, price range)
- `GET /products/{id}` - Get product details
- `GET /products/{id}/price` - Calculate price based on quantity

## Technology Stack

- **.NET 8.0**: Modern web API framework
- **AWS DynamoDB**: NoSQL database for trader data
- **AWS SDK**: Integration with AWS services
- **OpenAPI/Swagger**: API documentation
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework for tests

## Getting Started

### Prerequisites

- .NET 8.0 or later
- Visual Studio 2022 or VS Code
- AWS CLI configured with valid credentials
- AWS account with DynamoDB access

### AWS Setup

1. **Configure AWS CLI**:
   ```bash
   aws configure
   ```
   - Region: `af-south-1` (Cape Town)
   - Provide your AWS Access Key and Secret Key

2. **DynamoDB Table**: The `Traders` table should be created in `af-south-1` region

### Running the Application

1. Clone the repository
2. Navigate to the solution directory
3. Restore packages:
   ```bash
   dotnet restore
   ```
4. Run the Traders service:
   ```bash
   cd SpazaLink.Services.Traders
   dotnet run
   ```
5. Run tests:
   ```bash
   dotnet test
   ```

The API will be available at `https://localhost:7xxx` (port varies)

## Project Structure

```
SpazaLink/
├── SpazaLink.Services.Traders/          # Trader management API
│   ├── Repositories/                    # Data access layer
│   │   ├── ITraderRepository.cs        # Repository interface
│   │   └── DynamoDbTraderRepository.cs # DynamoDB implementation
│   ├── Program.cs                      # API endpoints and configuration
│   └── appsettings.json               # Configuration settings
├── SpazaLink.Services.Inventory/        # Product catalog API
│   ├── Repositories/                    # Data access layer
│   │   ├── IProductRepository.cs       # Product repository interface
│   │   └── DynamoDbProductRepository.cs # DynamoDB implementation
│   └── Program.cs                      # API endpoints and configuration
├── SpazaLink.Shared/                   # Shared components
│   ├── Models/                         # Domain models
│   │   ├── Trader.cs                  # Trader entity
│   │   ├── Product.cs                 # Product catalog entity
│   │   └── Supplier.cs                # Supplier network entity
│   ├── DTOs/                          # Data transfer objects
│   ├── Enums/                         # Enumerations
│   │   ├── TraderEnums.cs            # Trader-related enums
│   │   └── SupplierTypeEnums.cs      # Supplier type classifications
│   ├── Constants/                     # Application constants
│   ├── Exceptions/                    # Custom exceptions
│   └── Validators/                    # Input validation
├── SpazaLink.Services.Traders.Tests/   # Unit tests
│   └── Repositories/                   # Repository tests
├── SpazaLink.Services.Inventory.Tests/ # Inventory service tests
│   ├── Repositories/                   # Repository tests
│   └── Integration/                    # Integration tests
└── README.md
```

## Data Models

### Trader
Core trader entity with business information, location, status, and verification details.

### Product
Product catalog entity with pricing, categories, supplier information, and stock management.

### Supplier
Supplier network entity with contact details, delivery areas, and supplier type classification.

### CreateTraderRequest
DTO for trader registration containing required business and contact information.

## Status Management

- **Pending Verification**: Newly registered traders
- **Active**: Verified and operational
- **Suspended**: Temporarily inactive
- **Inactive**: Not currently operating
- **Banned**: Permanently removed

## Tier System

- **Bronze**: New traders (< R5k monthly turnover)
- **Silver**: R5k - R20k monthly turnover
- **Gold**: R20k - R50k monthly turnover
- **Platinum**: > R50k monthly or top 10%

## Supplier Types

- **Wholesaler**: Large retailers (Makro, Metro)
- **Distributor**: Brand-specific distributors
- **Manufacturer**: Direct from factory suppliers
- **Local Producer**: Small local producers

## Testing

The project includes comprehensive unit tests:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Code Quality

- **XML Documentation**: All public APIs documented
- **Repository Pattern**: Clean separation of concerns
- **Dependency Injection**: Testable and maintainable code
- **Error Handling**: Proper exception handling and validation
- **Unit Testing**: High test coverage with mocked dependencies

## Contributing

1. Fork the repository
2. Create a feature branch
3. Write tests for new functionality
4. Ensure all tests pass
5. Add XML documentation for public APIs
6. Submit a pull request

## License

This project is licensed under the MIT License.