# SpazaLink

A digital platform connecting local spaza shops, tuck shops, and street vendors with their communities.

## Overview

SpazaLink is a microservices-based solution designed to digitize and connect small-scale traders (spaza shops, tuck shops, street vendors) in South African communities. The platform enables trader registration, verification, and management through a RESTful API.

## Architecture

- **SpazaLink.Services.Traders**: Web API service for trader management
- **SpazaLink.Shared**: Common models, DTOs, enums, and utilities

## Features

- Trader registration and profile management
- Multi-tier trader classification (Bronze, Silver, Gold, Platinum)
- Support for various trader types (Spaza Shop, Street Vendor, Tuck Shop, Home Business)
- Verification system with KYC capabilities
- Multiple payment method support
- Multi-channel notifications (SMS, WhatsApp, Email)

## Trader Types

- **Spaza Shop**: Small convenience stores
- **Street Vendor**: Mobile/market stall traders
- **Tuck Shop**: School/office-based vendors
- **Home Business**: Home-operated businesses

## API Endpoints

### Traders Service

- `POST /createtrader` - Register a new trader
- `GET /weatherforecast` - Sample endpoint

## Getting Started

### Prerequisites

- .NET 8.0 or later
- Visual Studio 2022 or VS Code

### Running the Application

1. Clone the repository
2. Navigate to the solution directory
3. Restore packages:
   ```
   dotnet restore
   ```
4. Run the Traders service:
   ```
   cd SpazaLink.Services.Traders
   dotnet run
   ```

The API will be available at `https://localhost:7xxx` (port varies)

## Project Structure

```
SpazaLink/
├── SpazaLink.Services.Traders/     # Trader management API
│   ├── Program.cs                  # API endpoints and configuration
│   └── appsettings.json           # Configuration settings
├── SpazaLink.Shared/              # Shared components
│   ├── Models/                    # Domain models
│   ├── DTOs/                      # Data transfer objects
│   ├── Enums/                     # Enumerations
│   ├── Constants/                 # Application constants
│   ├── Exceptions/                # Custom exceptions
│   └── Validators/                # Input validation
└── README.md
```

## Data Models

### Trader
Core trader entity with business information, location, status, and verification details.

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

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License.