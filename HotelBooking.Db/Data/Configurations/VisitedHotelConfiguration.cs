using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data.Configurations;

public class VisitedHotelConfiguration : IEntityTypeConfiguration<VisitedHotel>
{
    public void Configure(EntityTypeBuilder<VisitedHotel> builder)
    {
        builder.HasKey(vh => vh.Id);

        builder.HasOne(vh => vh.User)
            .WithMany(u => u.VisitedHotels)
            .HasForeignKey(vh => vh.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vh => vh.Hotel)
            .WithMany(h => h.VisitedHotels)
            .HasForeignKey(vh => vh.HotelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}