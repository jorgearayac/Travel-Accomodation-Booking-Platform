using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Repositories;

/// <summary>
/// Repository implementation for managing city data in the database.
/// </summary>
public class CityRepository : Repository<City>, IRepository<City>
{
    public CityRepository(HotelBookingDbContext context) : base(context) { }
}