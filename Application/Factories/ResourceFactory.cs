using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
	public class ResourceFactory : IResourceFactory
	{
		readonly IResourceRepository _repository;

		public ResourceFactory(IResourceRepository repository)
		{
			_repository = repository;
		}
		public async Task<IResult<Resource>> CreateResourceAsync(Resource resource)
		{
			if (await _repository.GetResourceByResourceNameAsync(resource.Name) != null)
			{
                return Result<Resource>.Error(resource, new Exception("Ressourcen eksisterer allerede."));
            }
			else
			{

				return Result<Resource>.Success(resource);
			}
		}
	}
}
