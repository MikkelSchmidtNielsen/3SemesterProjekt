using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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

		// CREATE
        public async Task<IResult<Booking>> CreateBookingAsync(Booking booking)
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

        public async Task<IResult<IEnumerable<Booking>>> GetActiveBookingsWithMissingCheckInsAsync()
        {
            try
            {
                IEnumerable<Booking> bookings = await _db.Bookings.Where(b => !b.isCheckedIn).Include(b => b.Guest).Include(b => b.Resource).ToListAsync();

                return Result<IEnumerable<Booking>>.Success(bookings);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Booking>>.Error(null, ex);
            }
        }
    }
}
