using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System.Net.Http.Headers;

namespace Application.Factories
{
	public class ResourceFactory : IResourceFactory
	{
		readonly IResourceRepository _repository;

		public ResourceFactory(IResourceRepository repository)
		{
			_repository = repository;
		}
		public async Task<IResult<Resource>> CreateResourceAsync(CreateResourceFactoryDto dto)
		{
			// Gets a IEnumerable of all resources with dtos Name
			var resourceNameResult = await _repository.GetAllResourcesAsync(new ReadResourceListQueryDto() { Name = dto.Name});

			// Gets a IEnumerable of all resources with dtos Location
			var resourceLocationResult = await _repository.GetAllResourcesAsync(new ReadResourceListQueryDto() { Location = dto.Location });

			// If a resource with either Name or Location exist. Return error
			if (resourceNameResult.GetSuccess().OriginalType.Any() || resourceLocationResult.GetSuccess().OriginalType.Any())
			{
				return Result<Resource>.Error(null, new Exception("En ressource eksistere allerede med det navn eller placering")).SetStatusCode(System.Net.HttpStatusCode.Conflict);
			}
			else
			{
                Resource resource = new Resource(dto);
                return Result<Resource>.Success(resource).SetStatusCode(System.Net.HttpStatusCode.OK);
			}
		}
	}
}
