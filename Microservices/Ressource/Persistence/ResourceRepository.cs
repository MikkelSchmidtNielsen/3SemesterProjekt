using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Persistence.Repository
{
	public class ResourceRepository : IResourceRepository
	{
		private readonly MySqlServerDbContext _db;

		public ResourceRepository(MySqlServerDbContext db)
		{
			_db = db;
		}

		// Create
        public async Task<IResult<Resource>> AddResourceToDBAsync(Resource resource)
        {
            try
            {
                await _db.Resources.AddAsync(resource);
                await _db.SaveChangesAsync();

                return Result<Resource>.Success(resource).SetStatusCode(System.Net.HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Result<Resource>.Error(resource, ex).SetStatusCode(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    
        // READ
        public async Task<IResult<Resource>> GetResourceByIdAsync(int id)
        {
			try
			{
				Resource resource = await _db.Resources
					.FirstAsync(x => x.Id == id);

				return Result<Resource>.Success(resource);
			}
			catch (Exception ex)
			{
                // Returns invalid resource with exception
                return Result<Resource>.Error(originalType: null!, exception: ex);
			}
		}

        // LIST
        public async Task<IResult<IEnumerable<Resource>>> GetAllResourcesAsync(ReadResourceListQueryDto criteria)
        {
			// query is a holder of all criteria
			IQueryable<Resource> query = _db.Resources.AsQueryable();

			if (!string.IsNullOrWhiteSpace(criteria.Name))
				query = query.Where(r => r.Name.Contains(criteria.Name));

			if (criteria.Type != null && criteria.Type.Any())
				query = query.Where(r => criteria.Type.Contains(r.Type));

			if (criteria.Location.HasValue)
				query = query.Where(r => r.Location == criteria.Location.Value);

			if (criteria.IsAvailable.HasValue)
				query = query.Where(r => r.IsAvailable == criteria.IsAvailable.Value);

			if (criteria.MinPrice.HasValue)
				query = query.Where(r => r.BasePrice >= criteria.MinPrice.Value);

			if (criteria.MaxPrice.HasValue)
				query = query.Where(r => r.BasePrice <= criteria.MaxPrice.Value);
	
			// query.ToList searches the database with given criteria
			return Result<IEnumerable<Resource>>.Success(await query.ToListAsync());
		}
    }
}
