using HotelBooking.Db.Data;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Db.Repositories;

public class HotelRepository : Repository<Hotel>, IHotelRepository
{
    public HotelRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<Hotel>> GetAllWithRoomsAsync()
    {
        return await _dbSet.Include(h => h.Rooms)
            .ToListAsync();
    }

    public async Task<Hotel?> GetByIdWithRoomsAsync(int id)
    {
        return await _dbSet.Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<(IEnumerable<Hotel> Items, int TotalCount)> GetPaginatedWithRoomsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet
            .Include(h => h.Rooms)
            .OrderBy(h => h.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<Hotel> Items, int TotalCount)> SearchAsync(
    string? query,
    decimal? minPrice,
    decimal? maxPrice,
    int? starRate,
    string? roomType,
    int adults,
    int children,
    int pageNumber,
    int pageSize)
    {
        var queryable = _dbSet
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .AsQueryable();

        // text search: hotel name or city name
        if (!string.IsNullOrWhiteSpace(query))
        {
            queryable = queryable.Where(h =>
                h.Name.Contains(query) ||
                h.City.Name.Contains(query));
        }

        // price range
        if (minPrice.HasValue)
        {
            queryable = queryable.Where(h => h.PricePerNight >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            queryable = queryable.Where(h => h.PricePerNight <= maxPrice.Value);
        }

        // star rating
        if (starRate.HasValue)
        {
            queryable = queryable.Where(h => h.StarRate >= starRate.Value);
        }

        // room type
        if (!string.IsNullOrWhiteSpace(roomType))
        {
            var parsedRoomType = Enum.Parse<RoomType>(roomType);
            queryable = queryable.Where(h => h.Rooms.Any(r => r.RoomType == parsedRoomType));
        }

        // room availability
        queryable = queryable.Where(h => h.Rooms.Any(r =>
            r.Availability &&
            r.AdultCapacity >= adults &&
            r.ChildCapacity >= children));

        // total count before pagination
        var totalCount = await queryable.CountAsync();

        // pagination
        var items = await queryable
            .OrderBy(h => h.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
