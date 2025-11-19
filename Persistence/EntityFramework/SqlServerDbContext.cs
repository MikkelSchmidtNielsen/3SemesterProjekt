using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.EntityFramework.EfModelConfigurations;

namespace Persistence.EntityFramework
{
    public class SqlServerDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Guest> Guests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CampingDB;Integrated Security=SSPI;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
        }
    }
}