using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.API.Models;

namespace HotelBooking.API.Data.Configurations;

public class HotelImageConfiguration : IEntityTypeConfiguration<HotelImage>
{
    public void Configure(EntityTypeBuilder<HotelImage> builder)
    {
        builder.HasKey(hi => hi.Id);

        builder.Property(hi => hi.ImageUrl).HasMaxLength(1000);
        builder.Property(hi => hi.Caption).HasMaxLength(500);
        
        builder.HasOne(hi => hi.Hotel)
            .WithMany(h => h.HotelImages)
            .HasForeignKey(hi => hi.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}