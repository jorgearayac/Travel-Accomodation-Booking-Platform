using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.API.Models;

namespace HotelBooking.API.Data.Configurations;

public class BookingRoomConfiguration : IEntityTypeConfiguration<BookingRoom>
{
    public void Configure(EntityTypeBuilder<BookingRoom> builder)
    {
        builder.HasKey(br => br.Id);

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
