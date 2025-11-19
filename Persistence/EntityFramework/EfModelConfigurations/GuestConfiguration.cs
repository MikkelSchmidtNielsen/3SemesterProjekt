using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityFramework.EfModelConfigurations
{
    internal class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("Guest");
            builder.HasKey(guest => guest.Id);

            // Relations
            builder.HasMany(guest => guest.Booking)
                   .WithOne(booking => booking.Guest)
                   .HasForeignKey(booking => booking.GuestId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}