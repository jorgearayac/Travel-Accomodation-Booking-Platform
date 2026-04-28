# Travel & Accomodation Booking Platform

## Hotel Booking API
A RESTful API for an online hotel booking system built with ASP.NET Core Web API, Entity Framework Core, and SQL Server. This project covers user authentication, hotel search with filters, room booking, and admin management.

## Tech Stack
- **Framework:** ASP.NET Core Web API (.NET 10)
- **Database:** SQL Server (LocalDB)
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Tokens
- **Password Hashing:** BCrypt
- **Testing:** xUnit + Moq
- **Documentation:** Swagger / OpenAPI

## Project Structure
```
HotelBooking.API/        - API layer (controllers, services, DTOs middleware)
- Controllers/           - API endpoints
- DTOs/                  - Data Transfer Objects (request/response models)
- Extensions/            - Program extension methods
- Interfaces/            - Service contracts
- Middleware/            - Global exception handler
- Services/              - Business logic
- Program.cs             - Application entry point

HotelBooking.Db/         - Domain layer (entities, repositories, database)
- Data/                  - DbContext and entity configurations
   - Configurations/     - EF Core entity type configurations
- Enums/                 - Domain enumerations
- Interfaces/            - Repository contracts
- Models/                - Entity models
- Repositories/          - Data access implementations

HotelBooking.Tests/      - Unit tests
- ServiceTests/          - Service layer tests
```

## How to Run

### Prerequisites
- .NET 10 SDK
- Visual Studio 2022+ or VS Code
- SQL Server LocalDB (included with Visual Studio)

### Setup
Clone the repository:
```bash
git clone <repository-url>
cd Travel-Accomodation-Booking-Platform
```

Update the connection string in `appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=HotelBookingDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

Add JWT settings in `appsettings.Development.json`:
```json
{
  "Jwt": {
    "Secret": "secretkey-that-is-super-long-and-secure-for-this-project"
  }
}
```

Apply database migrations:
```bash
dotnet ef database update --project HotelBooking.Db --startup-project HotelBooking.API
```

Run the application:
```bash
dotnet run --project HotelBooking.API
```

Open Swagger UI at `https://localhost:{port}/swagger`

### Default Admin Account
The database is seeded with an admin user:
- **Username:** admin
- **Password:** adminpassword

### Registration & Login
You can register you own user with POST `/api/auth/register` in the format:
```json
{
    "username": "<your-username>",
    "password": "<your-password>"
}
```
Then login in POST `/api/auth/login` with your credentials. A JWT token will be provided so you can authenticate.
## API Endpoints

### Authentication
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/register` | Register a new user | None |
| POST | `/api/auth/login` | Login and receive JWT token | None |

### Cities
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/cities` | Get all cities (paginated) | User |
| GET | `/api/cities/{id}` | Get city by Id | User |
| POST | `/api/cities` | Create a city | Admin |
| PUT | `/api/cities/{id}` | Update a city | Admin |
| DELETE | `/api/cities/{id}` | Delete a city | Admin |

### Hotels
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/hotels` | Get all hotels (paginated) | User |
| GET | `/api/hotels/{id}` | Get hotel by Id | User |
| GET | `/api/hotels/{id}/details` | Get full hotel details (images, reviews, rooms) | User |
| GET | `/api/hotels/{id}/images` | Get hotel images | User |
| GET | `/api/hotels/search` | Search hotels with filters | User |
| POST | `/api/hotels` | Create a hotel | Admin |
| PUT | `/api/hotels/{id}` | Update a hotel | Admin |
| DELETE | `/api/hotels/{id}` | Delete a hotel | Admin |
| POST | `/api/hotels/{id}/images` | Add hotel image | Admin |
| PUT | `/api/hotels/images/{imageId}` | Update hotel image | Admin |
| DELETE | `/api/hotels/images/{imageId}` | Delete hotel image | Admin |

### Rooms
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/rooms` | Get all rooms (paginated) | User |
| GET | `/api/rooms/{id}` | Get room by Id | User |
| POST | `/api/rooms` | Create a room | Admin |
| PUT | `/api/rooms/{id}` | Update a room | Admin |
| DELETE | `/api/rooms/{id}` | Delete a room | Admin |

### Booking
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/booking/{id}` | Get booking by Id | User |
| GET | `/api/booking/user` | Get current user's bookings | User |
| POST | `/api/booking` | Create a booking | User |

### Reviews
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/reviews/hotel/{hotelId}` | Get reviews for a hotel | User |
| POST | `/api/reviews` | Create a review | User |
| DELETE | `/api/reviews/{id}` | Delete own review | User |

### Featured Deals
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/featured-deals` | Get all featured deals | User |
| GET | `/api/featured-deals/{id}` | Get featured deal by Id | User |
| POST | `/api/featured-deals` | Create a featured deal | Admin |
| PUT | `/api/featured-deals/{id}` | Update a featured deal | Admin |
| DELETE | `/api/featured-deals/{id}` | Delete a featured deal | Admin |

### Home Page
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/home/featured-deals` | Top 5 featured deals | User |
| GET | `/api/home/recently-booked` | User's last 5 booked hotels | User |
| GET | `/api/home/trending-destinations` | Top 5 cities by bookings | User |

## Search Filters
The hotel search endpoint supports the following query parameters:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| query | string | null | Search by hotel or city name |
| minPrice | decimal | null | Minimum price per night |
| maxPrice | decimal | null | Maximum price per night |
| starRate | int | null | Minimum star rating |
| roomType | string | null | Room type (Budget, Luxury, Boutique) |
| checkInDate | DateTime | null | Check-in date |
| checkOutDate | DateTime | null | Check-out date |
| adults | int | 2 | Number of adults |
| children | int | 0 | Number of children |
| rooms | int | 1 | Number of rooms |
| pageNumber | int | 1 | Page number |
| pageSize | int | 10 | Items per page |

Example: `GET /api/hotels/search?query=Rancagua&minPrice=50&maxPrice=200&starRate=4&roomType=Luxury`

## Authentication
The API uses JWT Bearer tokens. To access protected endpoints:

1. Register or login to receive a token
2. Include the token in the Authorization header: `Authorization: Bearer <your-token>`
3. Tokens expire after 1 hour

### Roles
- **User** — can browse hotels, make bookings, write reviews
- **Admin** — can manage cities, hotels, rooms, featured deals

## Error Handling
The API uses a global exception handler with `IExceptionHandler` that returns standardized `ProblemDetails` responses:

```json
{
  "status": 404,
  "title": "City with Id 999 not found.",
  "instance": "/api/cities/999",
  "timestamp": "2025-08-01T12:00:00Z"
}
```

## Architecture
The project follows a **Layered Architecture** with DDD principles:

- Controller (API)
  - Service (Business logic)
    - Repository (Data access)
      - DbContext (Database)

Key features:
- **Generic Repository Pattern:** base CRUD operations shared across all entities
- **Dependency Injection:** all layers communicate through interfaces
- **DTO Pattern:** domain models are never exposed directly through the API
- **Price captured:** room prices are captured at booking time (PriceAtBooking)

## Testing
Run the tests:
```bash
dotnet test
```

Tests cover:
- **AuthService:** login, registration, duplicate user checks
- **BookingService:** date validation, price calculation, room availability
- **ReviewService:** duplicate prevention, ownership validation

## Entity Relationships
- A **City** has many Hotels
- A **Hotel** belongs to a City and has many Rooms, Images, Reviews, and Featured Deals
- A **Room** belongs to a Hotel and can be in many Bookings (through BookingRoom)
- A **Booking** belongs to a User and contains multiple Rooms (through BookingRoom)
- A **Review** belongs to a User and a Hotel (one review per user per hotel)
- A **BookingRoom** snapshots the room price at booking time
