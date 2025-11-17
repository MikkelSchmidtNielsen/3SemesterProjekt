using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
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
		public async Task<IResult<Resource>> CreateResourceAsync(CreateResourceDto dto)
		{
			Resource resourceAlreadyInDatabase = await _repository.GetResourceByResourceNameAsync(dto.Name);

			if (resourceAlreadyInDatabase != null)
			{
                return Result<Resource>.Error(resourceAlreadyInDatabase, new Exception("Ressourcen eksisterer allerede."));
            }
			else
			{
                Resource resource = new Resource(dto);
				await _repository.AddResourceToDBAsync(resource);
                return Result<Resource>.Success(null);
			}
		}
	}
}
