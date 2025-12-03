using Application.ApplicationDto.Query;
using Common.ResultInterfaces;

namespace Application.ServiceInterfaces.Query
{
    public interface IReadResourceByIdQuery
    {
        Task<IResult<ReadResourceByIdQueryResponseDto>> ReadResourceByIdAsync(int id);
    }
}
