using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityFramework.EfModelConfigurations;

namespace Persistence.EntityFramework
{
    public class SqlServerDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; } 
        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Camping;Integrated Security=SSPI;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
        }
    }
}
