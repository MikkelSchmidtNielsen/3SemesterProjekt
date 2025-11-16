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

        // READ
        public async Task<Resource> GetResourceByIdAsync(int id)
        {
            Resource? resource;

            try
            {
                resource = await _db.Resources
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                throw new Exception("Kunne ikke finde");
            }

            return resource;
        }

        // LIST
        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            IEnumerable<Resource> resources = await _db.Resources
                .ToListAsync();

            return resources;
        }

        
    }
}
