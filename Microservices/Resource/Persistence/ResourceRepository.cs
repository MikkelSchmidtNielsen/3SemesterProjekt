using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Common;
using Common.CustomExceptions;
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
        public async Task<IResult<Resource>> CreateAsync(Resource resource)
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
    
        // READ
        public async Task<IResult<Resource?>> GetByIdAsync(int id)
        {
			Resource? resource = await _db.Resources
				.FirstOrDefaultAsync(x => x.Id == id);

			return Result<Resource>.Success(resource);
		}

        // LIST
        public async Task<IResult<IEnumerable<Resource>>> GetAllAsync(ReadResourceListQueryDto criteria)
        {
			// query is a holder of all criteria
			IQueryable<Resource> query = _db.Resources.AsQueryable();

			if (!string.IsNullOrWhiteSpace(criteria.Name))
				query = query.Where(r => r.Name.Contains(criteria.Name));

			if (criteria.Type is not null)
			{
				// If Type only contains null or empty strings
				criteria.Type = criteria.Type
								.Where(t => !string.IsNullOrWhiteSpace(t))
								.ToList();

				if (criteria.Type.Any())
					query = query.Where(r => criteria.Type.Contains(r.Type));
			}

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

		public async Task<IResult<Resource>> UpdateAsync(Resource resource)
		{
			// Saves the old rowversion as a local variable
			byte[] oldVersion = resource.RowVersion;

			// Increment the RowVersion
			resource.IncrementRowVersion();

			try
			{
				_db.Resources.Update(resource);

				// Notify which RowVersion should be compared
				// Needs to be the old one otherwise you'll compare the new RowVersion with Database RowVersion, which would always throw DbUpdateConcurrencyException
				_db.Entry(resource).OriginalValues["RowVersion"] = oldVersion;

				await _db.SaveChangesAsync();

				return Result<Resource>.Success(resource);
			}
			catch (DbUpdateConcurrencyException ex)
			{
				throw new ConflictException("En anden bruger har allerede opdateret produktet");
			}
		}
	}
}
