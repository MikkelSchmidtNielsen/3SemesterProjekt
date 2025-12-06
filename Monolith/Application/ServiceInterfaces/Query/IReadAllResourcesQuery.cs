using Application.ApplicationDto.Query;
using Common.ResultInterfaces;

namespace Application.ServiceInterfaces.Query
{
    public interface IReadAllResourcesQuery
    {
        public Task<IResult<IEnumerable<ReadResourceQueryResponseDto>>> ReadAllResourcesAsync(ResourceFilterDto filter);
    }
}
