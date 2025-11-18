using Application.Factories;
using Application.ServiceInterfaces.Command;
using Application.ApplicationDto.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DomainInterfaces;

namespace Application.Services.Command
{
    public class CreateResourceService : ICreateResourceService
    {
        private readonly IResourceFactory _factory;

        public CreateResourceService(IResourceFactory factory)
        {
            _factory = factory;
        }

        public async Task<IResult<Resource>> CreateResourceAsync(UICreateResourceDto dto)
        {
            var domainDto = Mapper.Map<CreateResourceDto>(dto);

            var checkIfNameAlreadyExists = await _factory.CreateResourceAsync(domainDto);

            return checkIfNameAlreadyExists;
        }
    }
}
