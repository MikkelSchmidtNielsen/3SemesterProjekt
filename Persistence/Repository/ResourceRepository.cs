using Application.RepositoryInterfaces;
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
	public class ResourceRepository : IResourceRepository
	{
		private readonly SqlServerDbContext _db;

		public ResourceRepository(SqlServerDbContext db)
		{
			_db = db;
		}


        public async Task<Resource> GetResourceByResourceNameAsync(string resourceName)
        {
			return await _db.Resources.FirstAsync(x => x.Name == resourceName);
        }
        public async Task<bool> AddResourceToDBAsync(Resource resource)
        {
            _db.Resources.Add(resource);
            return true;
        }
    }
}
