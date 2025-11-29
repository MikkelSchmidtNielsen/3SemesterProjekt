using Common.ResultInterfaces;
using Domain.Models;

namespace Application.ServiceInterfaces.Query
{
    public interface IResourceAllQuery
    {
        Task<IResult<IEnumerable<Resource>>> GetAllResourcesAsync();
    }
}
