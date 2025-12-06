# Minimal API

A minimal .NET 9.0 API project with a clean, organized folder structure.

## Project Structure

```
src/
├── Program.cs              # API configuration and endpoints
├── Controllers/            # API controllers (optional)
├── Models/                 # Data models and DTOs
├── Services/               # Business logic layer
├── Data/
│   └── Repositories/       # Data access layer
└── Middleware/             # Custom middleware
```

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio Code or Visual Studio

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`

### Test
```bash
dotnet test
```

## Architecture

- **Models**: Contains DTOs and domain entities
- **Services**: Business logic, interfaces, and implementations
- **Data/Repositories**: Data access layer for database operations
- **Controllers**: HTTP endpoint handlers (can use minimal APIs instead)
- **Middleware**: Custom request/response processing

## API Endpoints

Add your endpoints in `Program.cs`:

```csharp
app.MapGet("/api/products", handler)
   .WithName("GetProducts")
   .WithOpenApi();
```
