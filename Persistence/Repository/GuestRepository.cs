using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
	public class GuestRepository : IGuestRepository
	{
		private readonly SqlServerDbContext _db;

		public GuestRepository(SqlServerDbContext db)
		{
			_db = db;
		}

        public async Task<IResult<Guest>> GetGuestByEmailAsync(string email)
        {
            try
            {
                Guest guest = await _db.Guests.FirstAsync(x => x.Email == email);

                return Result<Guest>.Success(guest);
            }
            catch (Exception ex)
            {
                return Result<Guest>.Error(originalType: null!, exception: ex);
            }
        }
    }
}
