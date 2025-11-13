using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityFramework.EfModelConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
        }
    }
}
