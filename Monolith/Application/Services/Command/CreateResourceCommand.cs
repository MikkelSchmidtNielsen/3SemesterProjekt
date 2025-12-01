using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Application.Services.Command
{
    public class CreateResourceCommand : ICreateResourceCommand
    {
        private readonly 

        public CreateResourceCommand()
        {
            
        }

        public async Task<IResult<Resource>> CreateResourceAsync(UICreateResourceDto dto)
        {
            CreateResourceCommandDto commandDto = Mapper.Map<CreateResourceCommandDto>(dto);



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
