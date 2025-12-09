using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Application.InfrastructureDto;
using Common.ResultInterfaces;

namespace Application.ServiceInterfaces.Query
{
    public interface IResourceApiService
    {
        public Task<IResult<CreateResourceByApiResponseDto>> CreateResourceAsync(CreateResourceCommandDto dto);
        public Task<IResult<ReadResourceByIdByApiResponseDto>> ReadResourceByIdAsync(int id);
        public Task<IResult<IEnumerable<ReadResourceByApiQueryResponseDto>>> ReadAllResourcesAsync(ResourceFilterDto filter);
        public Task<IResult<UpdateResourceByApiResponseDto>> UpdateAsync(UpdateResourceCommandDto dto);
    }
}
