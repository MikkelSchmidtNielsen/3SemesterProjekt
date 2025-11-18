using Common.ResultInterfaces;
using Domain.Models;

namespace Application.ServiceInterfaces.Query
{
    public interface IResourceIdQuery
    {
        Task<IResult<Resource>> GetResourceByIdAsync(int id);
    }
}
