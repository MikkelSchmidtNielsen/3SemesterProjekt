using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryInterfaces
{
	public interface IResourceRepository
	{
		Task<IEnumerable<Resource>> GetAllResourcesAsync();
		Task<Resource> GetResourceByIdAsync(int id);
    }
}
