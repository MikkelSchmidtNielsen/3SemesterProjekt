using Application.ApplicationDto;
using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
    public class CreateResourceCommandHandler : ICreateResourceHandler
    {
        private readonly IResourceFactory _factory;
        private readonly IResourceRepository _repository;

        public CreateResourceCommandHandler(IResourceFactory factory, IResourceRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public async Task<IResult<ResourceResponseDto>> HandleAsync(CreateResourceCommandDto dto)
        {
            var domainDto = Mapper.Map<CreateResourceFactoryDto>(dto);

            // Creates Resource
            var newResource = await _factory.CreateResourceAsync(domainDto);

            // If error happends during Factory
            if (newResource.IsSucces() is false)
            {
                return Result<ResourceResponseDto>.Error(Mapper.Map<ResourceResponseDto>(dto), newResource.GetError().Exception!).SetStatusCode(HttpStatusCode.InternalServerError);
			}

            // Adds to database
			var result = await _repository.AddResourceToDBAsync(newResource.GetSuccess().OriginalType);

            // Returns Dto format based on repository result
			return result.IsSucces() ?
				Result<ResourceResponseDto>.Success(Mapper.Map<ResourceResponseDto>(result.GetSuccess().OriginalType)).SetStatusCode(result.StatusCode)
				: Result<ResourceResponseDto>.Error(Mapper.Map<ResourceResponseDto>(dto), result.GetError().Exception!).SetStatusCode(result.StatusCode);
		}
    }
}
