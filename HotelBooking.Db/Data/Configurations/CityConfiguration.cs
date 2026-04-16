using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasMaxLength(100);
        builder.Property(c => c.Country).HasMaxLength(100);
        builder.Property(c => c.PostOffice).HasMaxLength(20);
    }
}