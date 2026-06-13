# Branch Comparison: `dev` → `dev2`

**55 files changed — 1,124 insertions / 791 deletions**
**Commits:** `30a50e6` (code quality & security) · `cbbed82` (Result pattern refactor)

---

## Executive Summary

`dev2` is a quality and reliability hardening sprint on top of `dev`. The two headline changes are:

1. **Error handling architecture flip** — exception-throwing replaced with an explicit `Result<T>` return pattern across every service and controller.
2. **Data integrity hardened at the database level** — new SQL check constraints and unique indexes enforce rules that previously lived only in application code (or nowhere at all).

Everything else — validation annotations, transactions, performance tuning, logging, test rewrites — falls out of or supports those two decisions.

---

## 1. Result Pattern (Core Architecture Change)

### What changed

A new `Result` / `Result<T>` type (`HotelBooking.API/Common/Result.cs`) replaces C# exceptions as the primary error signal.

```csharp
// Before (dev)
public async Task<BookingResponse> GetBookingByIdAsync(int id)
{
    var booking = await _repo.GetByIdWithDetailsAsync(id);
    if (booking == null) throw new KeyNotFoundException("Booking not found.");
    return MapToResponse(booking);
}

// After (dev2)
public async Task<Result<BookingResponse>> GetBookingByIdAsync(int id, int userId)
{
    var booking = await _repo.GetByIdWithDetailsAsync(id);
    if (booking == null) return Result<BookingResponse>.NotFound("Booking not found.");
    if (booking.UserId != userId) return Result<BookingResponse>.Unauthorized("...");
    return Result<BookingResponse>.Success(MapToResponse(booking));
}
```

Four error types are modelled explicitly: `NotFound`, `Validation`, `Conflict`, `Unauthorized`.

### Why it matters

- Exceptions are costly for control-flow (stack unwinding) and make it impossible to distinguish "not found" from "server crash" without a catch-all handler.
- With `Result<T>`, the compiler forces callers to handle the error case. There is no silent swallow.
- HTTP mapping is now centralised in `ApiControllerBase.ToProblem()` — one place, no duplication across 8 controllers.

### Scope

Every service interface and implementation was updated: `AuthService`, `BookingService`, `CityService`, `FeaturedDealService`, `HotelService`, `HotelImageService`, `ReviewService`, `RoomService`.

---

## 2. Authorization Gap Closed in Booking

**Before:** `GetBookingByIdAsync(int id)` returned any booking by ID — any authenticated user could read another user's booking by guessing the ID.

**After:** The method signature is `GetBookingByIdAsync(int id, int userId)`. If the booking's `UserId` doesn't match, the service returns `Result.Unauthorized` → `403 Forbidden`.

This is a security fix, not just a refactor.

---

## 3. Database-Level Data Integrity

New SQL check constraints and unique indexes added via EF Core Fluent API. These enforce invariants even if application code is bypassed (direct DB writes, future bugs, migrations, seeding).

| Table | What was added | Effect |
|---|---|---|
| `Booking` | `CK_Booking_DateRange`: `CheckInDate < CheckOutDate` | Prevents zero-night or reversed bookings at the DB |
| `Room` | `CK_Room_PricePerNight`: `PricePerNight > 0` | Prevents free/negative-price rooms |
| `Hotel` | `CK_Hotel_PricePerNight`: `PricePerNight > 0` | Same for hotels |
| `FeaturedDeal` | `CK_FeaturedDeal_OriginalPrice` + `CK_FeaturedDeal_DiscountedPrice` | Discounted price must be positive and less than original |
| `BookingRoom` | Unique index on `(BookingId, RoomId)` | Prevents duplicate room entries per booking |
| `Review` | Unique index on `(UserId, HotelId)` | Enforces one-review-per-hotel-per-user at the DB (previously only a soft check in code) |
| `Room` | Unique index on `(HotelId, RoomNumber)` | Prevents duplicate room numbers within a hotel |

**Note:** These constraints require a new migration (`dotnet ef database update`) before running `dev2`.

---

## 4. Transaction Safety in Booking Creation

**Before:** Room availability was updated and the booking was saved in separate, non-atomic operations. A crash between the two steps would leave rooms marked unavailable but no booking recorded (or vice versa).

**After:** `CreateBookingAsync` wraps the entire operation — room lock, booking insert — in a database transaction with rollback on error.

```csharp
await using var transaction = await _context.Database.BeginTransactionAsync();
try {
    // ... validate, create booking, mark rooms unavailable
    await transaction.CommitAsync();
} catch {
    await transaction.RollbackAsync();
    throw;
}
```

The `BookingService` constructor now takes `HotelBookingDbContext` directly to support this.

---

## 5. Input Validation on All DTOs

Every request DTO gained `DataAnnotations` attributes. Examples:

| DTO | New rules |
|---|---|
| `RegisterRequest` | Username 3–50 chars, password 6–100 chars, valid email format, names capped at 100 chars |
| `HotelRequest` | StarRate `[Range(1,5)]`, PricePerNight `[Range(0.01, ∞)]`, Latitude `[-90,90]`, Longitude `[-180,180]` |
| `CreateBookingRequest` | SpecialRequests max 1000 chars, RoomIds `[MinLength(1)]` |
| `HotelSearchRequest` | Coordinate ranges enforced |

ASP.NET Core's model binding rejects invalid payloads before they reach a service method, so the validation is free — no service-layer branching needed.

---

## 6. Performance: Read Queries Use `AsNoTracking()`

All repository read methods that don't write data were updated to use `.AsNoTracking()`. This tells EF Core not to register retrieved entities with the change tracker, reducing memory overhead and speeding up read-heavy endpoints (listing hotels, searching, pagination).

---

## 7. Bug Fix: Safe Enum Parsing in Room Search

**Before:** `Enum.Parse<RoomType>(roomType)` would throw an unhandled exception if the query string contained an invalid value (e.g. `?roomType=invalid`).

**After:** `Enum.TryParse<RoomType>(roomType, ignoreCase: true, out var parsedRoomType)` — silently ignores unrecognised values and continues the query without that filter.

---

## 8. Review Service: Hotel Existence Validated

**Before:** `CreateReviewAsync` checked for duplicate reviews but not whether the target hotel existed. A review could be created for a non-existent hotel ID.

**After:** Hotel existence is validated first; missing hotel returns `Result.NotFound`.

---

## 9. Structured Logging Added

`ILogger<T>` injected into `AuthService`, `BookingService`, and `ReviewService`. Key events now produce structured log entries:

- User registered / logged in
- Booking created (with confirmation number and user ID)
- Review created / deleted

This feeds directly into any log aggregation or monitoring stack.

---

## 10. Test Suite Rewritten for Result Pattern

All `xUnit` tests in `BookingServiceTests`, `AuthServiceTests`, and `ReviewServiceTests` were updated:

```csharp
// Before
await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateBookingAsync(...));

// After
var result = await _service.CreateBookingAsync(...);
Assert.False(result.IsSuccess);
Assert.Equal(ErrorType.Validation, result.Error!.Type);
```

`BookingServiceTests` now implements `IDisposable` and provisions a real in-memory `HotelBookingDbContext` per test (using `Guid.NewGuid()` DB name for isolation), required by the new transaction-aware constructor.

---

## What Needs to Happen Before Merging / Deploying

1. **Run the migration** — new check constraints and unique indexes require `dotnet ef database update`.
2. **Verify no duplicate data exists** in the current DB that would violate the new unique indexes (`Review(UserId, HotelId)`, `Room(HotelId, RoomNumber)`, `BookingRoom(BookingId, RoomId)`). The migration will fail otherwise.
3. **Run the test suite** — `dotnet test` — to confirm nothing regressed.
4. **Update any API consumers** expecting `500`/exception responses for validation errors; they now receive `400`/`404`/`409`/`403` with a `ProblemDetails` body.

---

## File Change Summary

| Area | Files |
|---|---|
| New types | `Common/Result.cs`, `Controllers/ApiControllerBase.cs` |
| Services (all updated) | `AuthService`, `BookingService`, `CityService`, `FeaturedDealService`, `HotelImageService`, `HotelService`, `ReviewService`, `RoomService` |
| Interfaces (all updated) | All 8 service interfaces |
| Controllers (all updated) | `AuthController`, `BookingController`, `CitiesController`, `FeaturedDealsController`, `HomeController`, `HotelsController`, `ReviewsController`, `RoomsController` |
| DTOs (validation added) | `LoginRequest`, `RegisterRequest`, `CreateBookingRequest`, `CityRequest`, `FeaturedDealRequest`, `HotelRequest`, `HotelImageRequest`, `HotelSearchRequest`, `PaginationRequest`, `RoomRequest`, `CreateReviewRequest` |
| DB configurations | `BookingConfiguration`, `BookingRoomConfiguration`, `FeaturedDealConfiguration`, `HotelConfiguration`, `ReviewConfiguration`, `RoomConfiguration` |
| Repositories | `CityRepository`, `HotelRepository`, `ReviewRepository`, `UserRepository` (+ `AsNoTracking`), `BookingRepository`, `FeaturedDealRepository`, `HotelImageRepository` (minor) |
| Tests | `AuthServiceTests`, `BookingServiceTests`, `ReviewServiceTests` |
| Tooling | `CLAUDE.md` (project instructions added) |
