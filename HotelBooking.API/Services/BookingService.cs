using Azure.Core;
using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.DTOs.Rooms;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
    }

    public async Task<BookingResponse> GetBookingByIdAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdWithDetailsAsync(id);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with Id {id} not found.");
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
        // validate dates
        var nights = (request.CheckOutDate - request.CheckInDate).Days;
        if (nights <= 0)
        {
            throw new ArgumentException("Check-out date must be after check-in date.");
        }

        // get requested rooms and validate availability
        var rooms = await _roomRepository.GetRoomsByIdsAsync(request.RoomIds);
        if (rooms.Count() != request.RoomIds.Count)
        {
            throw new KeyNotFoundException("One or more rooms were not found.");
        }

        var unavailableRooms = rooms.Where(r => !r.Availability).ToList();
        if (unavailableRooms.Any())
        {
            throw new InvalidOperationException("One or more rooms are not available for the selected dates.");
        }

        // calculate total price
        var totalPrice = rooms.Sum(r => r.PricePerNight) * nights;

        // create confirmation number (8 chars from a new GUID)
        var confirmationNumber = Guid.NewGuid().ToString("N")[..8].ToUpper();

        // create the booking
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

        // create BookingRoom entries
        booking.BookingRooms = rooms.Select(room => new BookingRoom
        {
            RoomId = room.Id,
            PriceAtBooking = room.PricePerNight
        }).ToList();

        // mark rooms as unavailable
        foreach (var room in rooms)
        {
            room.Availability = false;
        }

        await _bookingRepository.AddAsync(booking);

        // reload with details
        var createdBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id);
        return MapToResponse(createdBooking!);
    }

    // method to get recently booked hotels for a user / user's recently visited hotels
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

    // Helper method to map Booking entity to BookingResponse DTO
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