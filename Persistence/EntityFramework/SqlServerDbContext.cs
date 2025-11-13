using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EntityFramework
{
    public class SqlServerDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations
        }
    }
}
