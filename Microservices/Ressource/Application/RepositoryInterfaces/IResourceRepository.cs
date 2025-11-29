using Common.ResultInterfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IResourceRepository
	{
		public Task<IResult<Resource>> GetResourceByResourceNameAsync(string resourceName);
		public Task<IResult<Resource>> GetResourceByLocationAsync(int resourceLocation);
		public Task<IResult<Resource>> AddResourceToDBAsync(Resource resource);
		Task<IResult<IEnumerable<Resource>>> GetAllResourcesAsync();
		Task<IResult<Resource>> GetResourceByIdAsync(int id);
	}
}
