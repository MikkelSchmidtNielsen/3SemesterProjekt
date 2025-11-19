using Application.ApplicationDto.Command;
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
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
    public class CreateResourceService : ICreateResourceService
    {
        private readonly IResourceFactory _factory;
        readonly IResourceRepository _repository;

        public CreateResourceService(IResourceFactory factory, IResourceRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public async Task<IResult<Resource>> CreateResourceAsync(UICreateResourceDto dto)
        {
            var domainDto = Mapper.Map<CreateResourceDto>(dto);

            var newResource = await _factory.CreateResourceAsync(domainDto);

            if (newResource.IsSucces())
            {
                var result = await _repository.AddResourceToDBAsync(newResource.GetSuccess().OriginalType);
                return result;
            }
            else
            {
                return newResource;
            }
        }
    }
}
