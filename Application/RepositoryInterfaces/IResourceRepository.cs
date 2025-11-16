using Common.ResultInterfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IResourceRepository
	{
		Task<IResult<IEnumerable<Resource>>> GetAllResourcesAsync();
		Task<IResult<Resource>> GetResourceByIdAsync(int id);
    }
}
