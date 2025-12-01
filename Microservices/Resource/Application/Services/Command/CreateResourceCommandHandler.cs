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
    public class CreateResourceCommandHandler : ICreateResourceCommandHandler
    {
        private readonly IResourceFactory _factory;
        private readonly IResourceRepository _repository;

		private CreateResourceCommandHandler() // Used for UnitTest
		{
		}

		public CreateResourceCommandHandler(IResourceFactory factory, IResourceRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }


        public async Task<IResult<ResourceResponseDto>> HandleAsync(CreateResourceCommandDto dto)
        {
            var domainDto = Mapper.Map<CreateResourceFactoryDto>(dto);

            // Creates Resource
            var newResource = await _factory.CreateAsync(domainDto);

            IResult<Resource> createdResource = await _repository.CreateAsync(newResource);

            return Result<ResourceResponseDto>.Success(Mapper.Map<ResourceResponseDto>(createdResource.GetSuccess().OriginalType));
		}
    }
}
