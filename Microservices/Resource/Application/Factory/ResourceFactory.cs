using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Common;
using Common.CustomExceptions;
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

		private ResourceFactory() { } // Used for UnitTesting

		public async Task<Resource> CreateAsync(CreateResourceFactoryDto dto)
		{
			// Gets a IEnumerable of all resources with dtos Name
			var resourceNameResult = await _repository.GetAllAsync(new ReadResourceListQueryDto() { Name = dto.Name});

			if (resourceNameResult.GetSuccess().OriginalType.Any()) throw new ConflictException("Der eksistere allerede en resource med det navn");

			// Gets a IEnumerable of all resources with dtos Location
			var resourceLocationResult = await _repository.GetAllAsync(new ReadResourceListQueryDto() { Location = dto.Location });

			if (resourceLocationResult.GetSuccess().OriginalType.Any()) throw new ConflictException("Der eksistere allerede en resource på den lokalation");

			return new Resource(dto);
		}
	}
}
