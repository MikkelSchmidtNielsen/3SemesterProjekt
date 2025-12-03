using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class MySqlServerDbContext : DbContext
    {
		public MySqlServerDbContext(DbContextOptions<MySqlServerDbContext> options)
		: base(options)
		{
            // Used so we can set up, when registration of DbContext in IoC container
		}

		public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User")
                .HasKey(user => user.Id);

            // Add property
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
