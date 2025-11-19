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

        public async Task<IResult<Resource>> GetResourceByResourceNameAsync(string resourceName)
        {
            Resource? resource = await _db.Resources.FirstOrDefaultAsync(x => x.Name == resourceName);

            if (resource is null)
            {
                return Result<Resource>.Error(resource, new Exception("En ressource med dette navn eksisterer ikke."));
            }
            else
            {
                return Result<Resource>.Success(resource);
            }
        }

        public async Task<IResult<Resource>> GetResourceByLocationAsync(int resourceLocation)
        {
            Resource? resource = await _db.Resources.FirstOrDefaultAsync(x => x.Location == resourceLocation);

            if (resource is null)
            {
                return Result<Resource>.Error(resource, new Exception("Der kunne ikke findes en ressource med det valgte pladsnr."));
            }
            else
            {
                return Result<Resource>.Success(resource);
            }
        }
        public async Task<IResult<Resource>> AddResourceToDBAsync(Resource resource)
        {
            try
            {
                await _db.Resources.AddAsync(resource);
                await _db.SaveChangesAsync();

                return Result<Resource>.Success(resource);
            }
            catch (Exception ex)
            {
                return Result<Resource>.Error(resource, ex);
            }
        }

    }
}
