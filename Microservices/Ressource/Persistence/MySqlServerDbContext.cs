using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class MySqlServerDbContext : DbContext
    {
        public MySqlServerDbContext(DbContextOptions<MySqlServerDbContext> options)
        : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>().ToTable("Resource")
                .HasKey(Resource => Resource.Id);
        }
    }
}
