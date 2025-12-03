using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.Services.Query
{
    public class ResourceIdQuery : IResourceIdQuery
    {
        private readonly IResourceRepository _repo;

        public ResourceIdQuery(IResourceRepository repo)
        {
            _repo = repo;
        }

        public async Task<IResult<Resource>> GetResourceByIdAsync(int id)
        {
            return await _repo.GetResourceByIdAsync(id);
        }
    }
}
