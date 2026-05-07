using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Data.Configurations;

public class FeaturedDealConfiguration : IEntityTypeConfiguration<FeaturedDeal>
{
    public void Configure(EntityTypeBuilder<FeaturedDeal> builder)
    {
        builder.HasKey(fd => fd.Id);

        builder.Property(fd => fd.OriginalPrice).HasPrecision(10, 2);
        builder.Property(fd => fd.DiscountedPrice).HasPrecision(10, 2);
        builder.Property(fd => fd.Description).HasMaxLength(200);

        builder.HasOne(fd => fd.Hotel)
            .WithMany(h => h.FeaturedDeals)
            .HasForeignKey(fd => fd.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(t => t.HasCheckConstraint("CK_FeaturedDeal_OriginalPrice", "[OriginalPrice] > 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_FeaturedDeal_DiscountedPrice", "[DiscountedPrice] > 0 AND [DiscountedPrice] < [OriginalPrice]"));
    }
}