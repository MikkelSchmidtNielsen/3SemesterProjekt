using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityFramework;

namespace Persistence.Repository
{
	public class GuestRepository : IGuestRepository
	{
		private readonly SqlServerDbContext _db;

		public GuestRepository(SqlServerDbContext db)
		{
			_db = db;
		}

		// CREATE
        public async Task<IResult<Guest>> CreateGuestAsync(Guest guest)
        {
			try
			{
				await _db.Guests.AddAsync(guest);
				await _db.SaveChangesAsync();

				return Result<Guest>.Success(guest);
			}
			catch (Exception ex)
			{
				return Result<Guest>.Error(guest, ex);
			}
		}

		// READ
        public async Task<IResult<Guest>> GetGuestByIdAsync(int id)
        {
			try
			{
				Guest? guest = await _db.Guests
					.FirstOrDefaultAsync(x => x.Id == id);

				if (guest == null)
				{
					// Returns invalid guest with exception
					return Result<Guest>.Error(
						originalType: null, 
						exception: new Exception($"Gæst med id {id} blev ikke fundet.")
					);
				}

				return Result<Guest>.Success(guest);
			}
			catch (Exception ex)
			{
				// Returns invalid guest with exception
				return Result<Guest>.Error(originalType: null, exception: ex);
			}
		}
    }
}
