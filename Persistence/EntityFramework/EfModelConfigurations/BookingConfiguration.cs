using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityFramework.EfModelConfigurations
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking");
            builder.HasKey(booking => booking.Id);

            // Relations
            builder.HasOne(booking => booking.Resource)
                   .WithMany(resource => resource.Bookings)                       
                   .HasForeignKey(booking => booking.ResourceId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
