using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityFramework.EfModelConfigurations;

namespace Persistence.EntityFramework
{
    public class SqlServerDbContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Camping;Integrated Security=SSPI;Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
        }
    }
}
