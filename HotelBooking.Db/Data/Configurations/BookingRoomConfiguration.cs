using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data.Configurations;

public class BookingRoomConfiguration : IEntityTypeConfiguration<BookingRoom>
{
    public void Configure(EntityTypeBuilder<BookingRoom> builder)
    {
        builder.HasKey(br => br.Id);

        builder.HasIndex(br => new { br.BookingId, br.RoomId }).IsUnique();

        builder.Property(br => br.PriceAtBooking).HasPrecision(10, 2);

        builder.HasOne(br => br.Booking)
            .WithMany(b => b.BookingRooms)
            .HasForeignKey(br => br.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(br => br.Room)
            .WithMany(r => r.BookingRooms)
            .HasForeignKey(br => br.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
