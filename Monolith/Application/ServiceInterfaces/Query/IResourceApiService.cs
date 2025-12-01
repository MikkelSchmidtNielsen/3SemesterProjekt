using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Common.ResultInterfaces;

namespace Application.ServiceInterfaces.Query
{
    public interface IResourceApiService
    {
        public Task<IResult<CreateResourceByApiResponseDto>> CreateResourceAsync(CreateResourceCommandDto dto);
    }
}
