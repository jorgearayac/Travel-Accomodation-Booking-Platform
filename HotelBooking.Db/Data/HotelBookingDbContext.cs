using Microsoft.EntityFrameworkCore;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data;

public class HotelBookingDbContext : DbContext
{
    public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingRoom> BookingRooms { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<HotelImage> HotelImages { get; set; }
    public DbSet<FeaturedDeal> FeaturedDeals { get; set; }
    public DbSet<VisitedHotel> VisitedHotels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // to be implemented in separate files for better organization
    }
}