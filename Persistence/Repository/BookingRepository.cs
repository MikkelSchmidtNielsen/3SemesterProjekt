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

        public async Task<IResult<Booking>> GuestCreateBookingAsync(Booking booking)
        {
            try
            {
                await _db.Bookings.AddAsync(booking);
                await _db.SaveChangesAsync();

                return Result<Booking>.Success(booking);
            }
            catch (Exception ex)
            {
                return Result<Booking>.Error(booking, ex);
            }
        }
    }
}
