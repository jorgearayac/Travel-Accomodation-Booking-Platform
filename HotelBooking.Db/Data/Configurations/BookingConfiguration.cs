using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.ConfirmationNumber).HasMaxLength(50);
        builder.Property(b => b.PaymentMethod).HasConversion<string>();
        builder.Property(b => b.SpecialRequests).HasMaxLength(1000);
        builder.Property(b => b.TotalPrice).HasPrecision(10, 2);
        builder.Property(b => b.PaymentStatus).HasConversion<string>();

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(t => t.HasCheckConstraint("CK_Booking_NumberOfAdults", "[NumberOfAdults] >= 1 AND [NumberOfAdults] <= 10"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Booking_NumberOfChildren", "[NumberOfChildren] >= 0 AND [NumberOfChildren] <= 10"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Booking_DateRange", "[CheckInDate] < [CheckOutDate]"));
    }
}