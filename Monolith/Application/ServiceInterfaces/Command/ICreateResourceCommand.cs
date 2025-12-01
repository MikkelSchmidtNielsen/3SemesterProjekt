using Common.ResultInterfaces;
using Application.ApplicationDto.Command;
using Application.ApplicationDto.Command.Responses;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateResourceCommand
    {
        public Task<IResult<CreateResourceUIResponseDto>> CreateResourceAsync(UICreateResourceDto dto);
    }
}
