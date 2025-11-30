using Application.ApplicationDto;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IResourceRepository
	{
		public Task<IResult<Resource>> AddResourceToDBAsync(Resource resource);
		Task<IResult<IEnumerable<Resource>>> GetAllResourcesAsync(ReadResourceListQueryDto criteria);
		Task<IResult<Resource>> GetResourceByIdAsync(int id);
	}
}
