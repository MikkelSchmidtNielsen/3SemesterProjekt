using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Persistence.EntityFramework;

namespace Persistence.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly SqlServerDbContext _db;

        public BookingRepository(SqlServerDbContext db)
        {
            _db = db;
        }
    }
}
