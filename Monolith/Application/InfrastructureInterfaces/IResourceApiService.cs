using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Common.ResultInterfaces;

namespace Application.InfrastructureInterfaces
{
    public interface IResourceApiService
    {
        public Task<IResult<CreateResourceByApiResponseDto>> CreateResourceAsync(CreateResourceCommandDto dto);
    }
}
