using Application.RepositoryInterfaces;
using Common.ResultInterfaces;
using Domain.Models;
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

        public Task<IResult<Guest>> GetGuestByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
