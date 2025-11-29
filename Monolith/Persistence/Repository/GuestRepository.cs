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

		public async Task<IResult<string>> CheckIfEmailIsAvailable(string email)
		{
			try
			{
				Guest? guest = await _db.Guests
					.FirstOrDefaultAsync(guest => guest.Email == email);

				if (guest == null)
				{
					return Result<string>.Success(email);
				}
				else
				{
					Exception ex = new Exception("Email already exist");
					return Result<string>.Conflict(email, guest.Email!, ex);
				}
			}
			catch (Exception ex)
			{
				return Result<string>.Error(email, ex);
			}
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
				Guest guest = await _db.Guests
					.FirstAsync(x => x.Id == id);

				return Result<Guest>.Success(guest);
			}
			catch (Exception ex)
			{
				// Returns invalid guest with exception
				return Result<Guest>.Error(originalType: null!, exception: ex);
			}
		}

        public async Task<IResult<Guest>> ReadGuestByEmailAsync(string email)
        {
            try
            {
                Guest guest = await _db.Guests.FirstAsync(x => x.Email == email);

                return Result<Guest>.Success(guest);
            }
            catch (Exception ex)
            {
                var custom = new InvalidOperationException("Vi kunne ikke finde dig i databasen", ex);

                return Result<Guest>.Error(originalType: null!, custom);
            }
        }
    }
}
