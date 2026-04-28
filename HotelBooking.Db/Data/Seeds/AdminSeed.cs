
using HotelBooking.Db.Enums;
using HotelBooking.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Db.Data.Seeds;

public class AdminSeed
{
    public static void Seed(ModelBuilder modelBuilder) // configure later
    {
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            PasswordHash = "$2a$11$oxotFrq0svCXVEdIFGN0q.T04.VG8V9TONwoCzg4gmvZlwcSGJIzK", // "adminpassword"
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@hotelbooking.com",
            Role = UserRole.Admin,
            CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
