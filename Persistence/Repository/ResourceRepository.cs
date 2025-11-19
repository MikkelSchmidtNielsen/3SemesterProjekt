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
	public class ResourceRepository : IResourceRepository
	{
		private readonly SqlServerDbContext _db;

		public ResourceRepository(SqlServerDbContext db)
		{
			_db = db;
		}

        public async Task<IResult<Resource>> GetResourceByIdAsync(int id)
        {
            try
            {
                Resource resource = await _db.Resources.FirstAsync(x => x.Id == id);

                return Result<Resource>.Success(resource);
            }
            catch (Exception ex)
            {
                return Result<Resource>.Error(originalType: null!, exception: ex);
            }
        }
        public async Task<IResult<IEnumerable<Resource>>> GetAllResourcesAsync()
        {
            try
            {
                IEnumerable<Resource> resources = await _db.Resources.ToListAsync();
                return Result<IEnumerable<Resource>>.Success(resources);
            }
            catch (Exception ex)
            {
                // Returns invalid list with exception
                return Result<IEnumerable<Resource>>.Error(originalType: null!, exception: ex);
            }
        }
    }
}
