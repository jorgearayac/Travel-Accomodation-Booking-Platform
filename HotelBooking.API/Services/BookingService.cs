using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.DTOs.Home;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Data;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly HotelBookingDbContext _context;
    private readonly ILogger<BookingService> _logger;

    public BookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        HotelBookingDbContext context,
        ILogger<BookingService> logger)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _context = context;
        _logger = logger;
    }

    public async Task<BookingResponse> GetBookingByIdAsync(int id, int userId)
    {
        var booking = await _bookingRepository.GetByIdWithDetailsAsync(id);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with Id {id} not found.");
        }

        if (booking.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only view your own bookings.");
        }

        return MapToResponse(booking);
    }

    public async Task<IEnumerable<BookingResponse>> GetBookingsByUserAsync(int userId)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId);
        return bookings.Select(MapToResponse);
    }

    public async Task<BookingResponse> CreateBookingAsync(int userId, CreateBookingRequest request)
    {
        if (request.CheckInDate.Date < DateTime.UtcNow.Date)
        {
            throw new ArgumentException("Check-in date cannot be in the past.");
        }

        var nights = (request.CheckOutDate - request.CheckInDate).Days;
        if (nights <= 0)
        {
            throw new ArgumentException("Check-out date must be after check-in date.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var rooms = await _roomRepository.GetRoomsByIdsAsync(request.RoomIds);
            if (rooms.Count() != request.RoomIds.Distinct().Count())
            {
                throw new KeyNotFoundException("One or more rooms were not found.");
            }

            var unavailableRooms = rooms.Where(r => !r.Availability).ToList();
            if (unavailableRooms.Any())
            {
                throw new InvalidOperationException("One or more rooms are not available for the selected dates.");
            }

            var totalPrice = rooms.Sum(r => r.PricePerNight) * nights;
            var confirmationNumber = Guid.NewGuid().ToString("N")[..12].ToUpper();

            var booking = new Booking
            {
                UserId = userId,
                ConfirmationNumber = confirmationNumber,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfAdults = request.NumberOfAdults,
                NumberOfChildren = request.NumberOfChildren,
                PaymentMethod = request.PaymentMethod,
                SpecialRequests = request.SpecialRequests,
                TotalPrice = totalPrice,
                PaymentStatus = PaymentStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            booking.BookingRooms = rooms.Select(room => new BookingRoom
            {
                RoomId = room.Id,
                PriceAtBooking = room.PricePerNight
            }).ToList();

            foreach (var room in rooms)
            {
                room.Availability = false;
            }

            await _bookingRepository.AddAsync(booking);
            await transaction.CommitAsync();

            _logger.LogInformation("Booking {ConfirmationNumber} created for user {UserId}", confirmationNumber, userId);

            var createdBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id);
            return MapToResponse(createdBooking!);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<RecentlyBookedHotelResponse>> GetRecentlyBookedHotelsAsync(int userId)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId);
        return bookings
            .SelectMany(b => b.BookingRooms.Select(br => new RecentlyBookedHotelResponse
            {
                HotelId = br.Room.Hotel.Id,
                HotelName = br.Room.Hotel.Name,
                CityName = br.Room.Hotel.City.Name,
                StarRate = br.Room.Hotel.StarRate,
                PricePerNight = br.Room.Hotel.PricePerNight,
                ThumbnailUrl = br.Room.Hotel.ThumbnailUrl,
                BookingDate = b.CreatedDate
            }))
            .DistinctBy(h => h.HotelId)
            .Take(5)
            .ToList();
    }

    private BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ConfirmationNumber = booking.ConfirmationNumber,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            NumberOfAdults = booking.NumberOfAdults,
            NumberOfChildren = booking.NumberOfChildren,
            PaymentMethod = booking.PaymentMethod.ToString(),
            SpecialRequests = booking.SpecialRequests,
            TotalPrice = booking.TotalPrice,
            PaymentStatus = booking.PaymentStatus.ToString(),
            Rooms = booking.BookingRooms.Select(br => new BookingRoomResponse
            {
                RoomId = br.RoomId,
                RoomNumber = br.Room.RoomNumber,
                HotelName = br.Room.Hotel.Name,
                RoomType = br.Room.RoomType.ToString(),
                PriceAtBooking = br.PriceAtBooking
            }).ToList(),
            CreatedDate = booking.CreatedDate,
            UpdatedDate = booking.UpdatedDate
        };
    }
}
