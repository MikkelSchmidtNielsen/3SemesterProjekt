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
			var resourceNameResult = await _repository.GetResourceByResourceNameAsync(dto.Name);
			var resourceLocationResult = await _repository.GetResourceByLocationAsync(dto.Location);

			if (resourceNameResult.IsSucces())
			{
                return Result<Resource>.Error(resourceNameResult.GetSuccess().OriginalType, new Exception("En ressource med dette navn eksisterer allerede."));
            }
			else if (resourceLocationResult.IsSucces())
			{
				return Result<Resource>.Error(resourceLocationResult.GetSuccess().OriginalType, new Exception("Der findes allerede en ressource på denne placering."));
			}
			else
			{
                Resource resource = new Resource(dto);
				await _repository.AddResourceToDBAsync(resource);
                return Result<Resource>.Success(resource);
			}
		}
	}
}
