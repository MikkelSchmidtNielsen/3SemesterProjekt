using Application.RepositoryInterfaces;
using Domain.Models;
using Persistence.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
	public class ResourceRepository : IResourceRepository
	{
		private readonly SqlServerDbContext _db;

		public ResourceRepository(SqlServerDbContext db)
		{
			_db = db;
		}
        public async Task<Resource> GetResourceByResourceNameAsync(string resourceName)
        {
			throw new NotImplementedException();
        }
    }
}
