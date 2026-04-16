using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name).HasMaxLength(200);
        builder.Property(h => h.Owner).HasMaxLength(100);
        builder.Property(h => h.Description).HasMaxLength(1000);
        builder.Property(h => h.PricePerNight).HasPrecision(10, 2);
        builder.Property(h => h.ThumbnailUrl).HasMaxLength(1000);

        builder.HasOne(h => h.City)
            .WithMany(c => c.Hotels)
            .HasForeignKey(h => h.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(t => t.HasCheckConstraint("CK_Hotel_StarRate", "[StarRate] >= 1 AND [StarRate] <= 5"));
    }
}