using Common.ResultInterfaces;
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
		public Task<IResult<Resource>> GetResourceByResourceNameAsync(string resourceName);
		public Task<IResult<Resource>> GetResourceByLocation(int resourceLocation);
		public Task<IResult<Resource>> AddResourceToDBAsync(Resource resource);
	}
}
