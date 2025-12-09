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

            modelBuilder.Entity<Resource>()
                .Property(r => r.RowVersion)
                .HasColumnType("BINARY(8)")
                .IsConcurrencyToken() // Says its a ConcurrencyToken and should be checked when updating
                .ValueGeneratedNever() // Make sure that EntityFramework doesn't think EF Core should create the new instance of ConcurryToken.
									   // EF Core can't create concurrency tokens for MySQL, so we do it manually
				.HasDefaultValue(BitConverter.GetBytes(1UL)); // Sets default value for RowVersion
		}
    }
}
