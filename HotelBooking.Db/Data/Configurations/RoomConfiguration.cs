using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoomNumber).HasMaxLength(20);
        builder.Property(r => r.RoomType).HasConversion<string>();
        builder.Property(r => r.Description).HasMaxLength(1000);
        builder.Property(r => r.PricePerNight).HasPrecision(10, 2);
        builder.Property(r => r.ThumbnailUrl).HasMaxLength(1000);

        builder.HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.HotelId, r.RoomNumber }).IsUnique();

        builder.ToTable(t => t.HasCheckConstraint("CK_Room_AdultCapacity", "[AdultCapacity] >= 1 AND [AdultCapacity] <= 10"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Room_ChildCapacity", "[ChildCapacity] >= 0 AND [ChildCapacity] <= 10"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Room_PricePerNight", "[PricePerNight] > 0"));
    }
}
