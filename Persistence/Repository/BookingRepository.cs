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
        public async Task<IResult<Booking>> AdminCreateBookingAsync(Booking booking)
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

        // READ
        public async Task<IResult<IEnumerable<Booking>>> GetActiveBookingsWithMissingCheckInsAsync()
        {
            try // Tries to retrieve all missed check-ins from today and earlier and return them as a successful result.
            {
                IEnumerable<Booking> bookings = await _db.Bookings
                    .Where(b => !b.isCheckedIn)
                    .Where(b => b.StartDate <= DateOnly.FromDateTime(DateTime.Now))
                    .Include(b => b.Guest)
                    .Include(b => b.Resource)
                    .ToListAsync();

                return Result<IEnumerable<Booking>>.Success(bookings);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Booking>>.Error(null, ex);
            }
        }

        public async Task<IResult<IEnumerable<Booking>>> GetFinishedBookingsWithMissingCheckOutsAsync()
        {
            try // Tries to retrieve all missed check-outs from today and earlier and return them as a successful result.
            {
                IEnumerable<Booking> bookings = await _db.Bookings
                    .Where(b => !b.isCheckedOut)
                    .Where(b => b.EndDate <= DateOnly.FromDateTime(DateTime.Now))
                    .Include(b => b.Guest)
                    .Include(b => b.Resource)
                    .ToListAsync();

                return Result<IEnumerable<Booking>>.Success(bookings);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Booking>>.Error(null, ex);
            }
        }
    }
}
