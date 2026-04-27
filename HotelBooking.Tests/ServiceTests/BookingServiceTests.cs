using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.Services;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Moq;

namespace HotelBooking.Tests.ServiceTests;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepo;
    private readonly Mock<IRoomRepository> _mockRoomRepo;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockBookingRepo = new Mock<IBookingRepository>();
        _mockRoomRepo = new Mock<IRoomRepository>();
        _bookingService = new BookingService(_mockBookingRepo.Object, _mockRoomRepo.Object);
    }

    [Fact]
    public async Task CreateBookingAsync_WithValidData_ReturnsBookingResponse()
    {
        // Arrange
        var testRooms = CreateTestRooms();
        _mockRoomRepo.Setup(repo => repo
            .GetRoomsByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(testRooms);

        _mockBookingRepo
            .Setup(repo => repo.AddAsync(It.IsAny<Booking>()))
            .ReturnsAsync((Booking b) => b);

        _mockBookingRepo
            .Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) =>
            {
                var booking = new Booking
                {
                    Id = 1,
                    UserId = 1,
                    ConfirmationNumber = "TEST1234",
                    CheckInDate = DateTime.UtcNow.Date.AddDays(10),
                    CheckOutDate = DateTime.UtcNow.Date.AddDays(13),
                    NumberOfAdults = 2,
                    NumberOfChildren = 0,
                    PaymentMethod = PaymentMethod.CreditCard,
                    TotalPrice = 900m,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    BookingRooms = testRooms.Select(r => new BookingRoom
                    {
                        RoomId = r.Id,
                        PriceAtBooking = r.PricePerNight,
                        Room = r,
                        Booking = null!
                    }).ToList()
                };
                return booking;
            });

        var bookingRequest = CreateTestBookingRequest();
        // Act
        var result = await _bookingService.CreateBookingAsync(1, bookingRequest);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal(bookingRequest.CheckInDate, result.CheckInDate);
        Assert.Equal(bookingRequest.CheckOutDate, result.CheckOutDate);
        Assert.Equal(bookingRequest.NumberOfAdults, result.NumberOfAdults);
        Assert.Equal(bookingRequest.NumberOfChildren, result.NumberOfChildren);
        Assert.Equal(bookingRequest.PaymentMethod.ToString(), result.PaymentMethod);
        Assert.Equal(testRooms.Count, result.Rooms.Count);
    }

    [Fact]
    public async Task CreateBookingAsync_WithCheckInInThePast_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateBookingRequest
        {
            CheckInDate = new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
            CheckOutDate = new DateTime(2025, 7, 5, 0, 0, 0, DateTimeKind.Utc),
            NumberOfAdults = 2,
            NumberOfChildren = 0,
            PaymentMethod = PaymentMethod.CreditCard,
            RoomIds = new List<int> { 1 }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _bookingService.CreateBookingAsync(1, request));
    }

    [Fact]
    public async Task CreateBookingAsync_WithCheckOutBeforeCheckIn_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateBookingRequest
        {
            CheckInDate = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc),
            CheckOutDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), // fixed dates
            NumberOfAdults = 2,
            NumberOfChildren = 0,
            PaymentMethod = PaymentMethod.CreditCard,
            RoomIds = new List<int> { 1 }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _bookingService.CreateBookingAsync(1, request));
    }

    [Fact]
    public async Task CreateBookingAsync_WithNonExistingRooms_ThrowsKeyNotFoundException()
    {
        // Arrange
        var request = CreateTestBookingRequest();
        _mockRoomRepo
            .Setup(repo => repo.GetRoomsByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(new List<Room>()); // empty list - no rooms found

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _bookingService.CreateBookingAsync(1, request));
    }

    [Fact]
    public async Task CreateBookingAsync_WithUnavailableRooms_ThrowsInvalidOperationException()
    {
        // Arrange
        var testRooms = CreateTestRooms();
        testRooms[0].Availability = false; // make one room unavailable

        _mockRoomRepo
            .Setup(repo => repo.GetRoomsByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(testRooms);

        var request = CreateTestBookingRequest();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _bookingService.CreateBookingAsync(1, request));
    }

    // total price calculation is correct (room price * number of nights)
    [Fact]
    public async Task CreateBookingAsync_CalculatesTotalPriceCorrectly()
    {
        // Arrange — Room 1 at $200, and Room 2 at $100 for 3 nights = $900
        var testRooms = CreateTestRooms();
        _mockRoomRepo
            .Setup(repo => repo.GetRoomsByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(testRooms);

        _mockBookingRepo
            .Setup(repo => repo.AddAsync(It.IsAny<Booking>()))
            .ReturnsAsync((Booking b) => b);

        _mockBookingRepo
            .Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Booking
            {
                Id = 1,
                UserId = 1,
                ConfirmationNumber = "TEST1234", // 8 length confirmation number
                CheckInDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                CheckOutDate = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc),
                NumberOfAdults = 2,
                NumberOfChildren = 0,
                PaymentMethod = PaymentMethod.CreditCard,
                TotalPrice = 900m,
                PaymentStatus = PaymentStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                BookingRooms = testRooms.Select(r => new BookingRoom
                {
                    RoomId = r.Id,
                    PriceAtBooking = r.PricePerNight,
                    Room = r
                }).ToList()
            });

        var request = CreateTestBookingRequest();

        // Act
        var result = await _bookingService.CreateBookingAsync(1, request);

        // Assert — $200 + $100 = $300 per night × 3 nights = $900
        Assert.Equal(900m, result.TotalPrice);
    }

    private List<Room> CreateTestRooms()
    {
        var hotel = new Hotel
        {
            Id = 1,
            Name = "Test Hotel",
            StarRate = 4,
            Owner = "Test Owner",
            Description = "Test Description",
            PricePerNight = 200m,
            ThumbnailUrl = "https://example.com/hotel.jpg",
            CityId = 1
        };

        return new List<Room>
        {
            new Room
            {
                Id = 1,
                HotelId = 1,
                RoomNumber = "101",
                RoomType = RoomType.Luxury,
                AdultCapacity = 2,
                ChildCapacity = 1,
                Description = "Luxury suite",
                PricePerNight = 200m,
                Availability = true,
                ThumbnailUrl = "https://example.com/room1.jpg",
                Hotel = hotel
            },
            new Room
            {
                Id = 2,
                HotelId = 1,
                RoomNumber = "102",
                RoomType = RoomType.Budget,
                AdultCapacity = 1,
                ChildCapacity = 0,
                Description = "Budget room",
                PricePerNight = 100m,
                Availability = true,
                ThumbnailUrl = "https://example.com/room2.jpg",
                Hotel = hotel
            }
        };
    }

    private CreateBookingRequest CreateTestBookingRequest()
    {
        return new CreateBookingRequest
        {
            CheckInDate = DateTime.UtcNow.Date.AddDays(10),
            CheckOutDate = DateTime.UtcNow.Date.AddDays(13),
            NumberOfAdults = 2,
            NumberOfChildren = 0,
            PaymentMethod = PaymentMethod.CreditCard,
            RoomIds = new List<int> { 1, 2 }
        };
    }
}

