using Application.ApplicationDto;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IResourceRepository
	{
		public Task<IResult<Resource>> CreateAsync(Resource resource);
		Task<IResult<IEnumerable<Resource>>> GetAllAsync(ReadResourceListQueryDto criteria);
		Task<IResult<Resource?>> GetByIdAsync(int id);
		Task<IResult<Resource>> UpdateAsync(Resource resource);
	}
}
