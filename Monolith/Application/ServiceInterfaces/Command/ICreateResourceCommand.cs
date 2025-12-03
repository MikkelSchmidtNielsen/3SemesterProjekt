using Common.ResultInterfaces;
using Application.ApplicationDto.Command;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateResourceCommand
    {
        public Task<IResult<CreateResourceUIResponseDto>> CreateResourceAsync(UICreateResourceDto dto);
    }
}
