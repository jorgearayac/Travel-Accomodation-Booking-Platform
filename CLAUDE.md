# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

There is no .sln file — use project-level commands:

```bash
# Build everything
dotnet build

# Run the API (Swagger at https://localhost:{port}/swagger)
dotnet run --project HotelBooking.API

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~BookingServiceTests"

# Run a single test method
dotnet test --filter "FullyQualifiedName~BookingServiceTests.CreateBooking_ShouldThrow_WhenCheckInDateIsInThePast"

# Apply database migrations
dotnet ef database update --project HotelBooking.Db --startup-project HotelBooking.API
```

## Architecture

Three-project layered architecture: **HotelBooking.API** → **HotelBooking.Db** → SQL Server (LocalDB).

- **HotelBooking.API** — Controllers, Services (business logic), DTOs, JWT auth setup, global exception handler. Services live here, not in the Db project.
- **HotelBooking.Db** — EF Core DbContext, entity models, repository implementations, Fluent API configurations, migrations, enums.
- **HotelBooking.Tests** — xUnit + Moq service-layer tests.

**Key patterns:**
- Generic `Repository<T>` base with specialized repository interfaces per entity
- All DI registration centralized in `Extensions/ServiceExtensions.cs` and `Extensions/AuthExtensions.cs`
- DTOs strictly separate API contracts from domain models — entities are never exposed directly
- `GlobalExceptionHandler` (implements `IExceptionHandler`) maps exceptions to `ProblemDetails` responses
- `BookingRoom` junction table captures `PriceAtBooking` — room prices are snapshotted at booking time

**Auth:** JWT Bearer tokens. Two roles: `User` and `Admin`. Token config in `appsettings.json` under `Jwt` section.

## Database

- Provider: SQL Server LocalDB
- Connection string: `appsettings.json` → `ConnectionStrings:DefaultConnection`
- Entity configurations: `HotelBooking.Db/Data/Configurations/`
- Migrations: `HotelBooking.Db/Migrations/`
- Seeded admin: username `admin`, password `adminpassword`

## Testing

Tests are in `HotelBooking.Tests/ServiceTests/`. They mock repositories via Moq and test service-layer business logic (auth flows, booking validation, review constraints). The InMemory EF Core provider is available as a dependency.
