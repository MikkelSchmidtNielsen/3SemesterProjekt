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

        public async Task<IResult<Guest>> CreateGuestAsync(Guest guest)
        {
            await _db.Guests.AddAsync(guest);

            await _db.SaveChangesAsync();

            return Result<Guest>.Success(guest);
        }

        public async Task<Guest> GetGuestByIdAsync(int id)
        {
            Guest? guest;

            try
            {
                guest = await _db.Guests
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                throw new Exception("Kunne ikke finde");
            }

            return guest;
        }
    }
}
