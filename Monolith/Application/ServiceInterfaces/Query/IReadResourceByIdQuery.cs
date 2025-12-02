using Application.ApplicationDto.Query;
using Application.ApplicationDto.Query.Responses;
using Common.ResultInterfaces;

namespace Application.ServiceInterfaces.Query
{
    public interface IReadResourceByIdQuery
    {
        Task<IResult<ReadResourceByIdQueryResponseDto>> ReadResourceByIdAsync(int id);
    }
}
