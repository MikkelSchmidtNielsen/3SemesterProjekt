using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
    public class ResourceIdQuery : IResourceIdQuery
    {
        private readonly IResourceRepository _repo;

        public ResourceIdQuery(IResourceRepository repo)
        {
            _repo = repo;
        }

        public async Task<Resource> GetResourceByIdAsync(int id)
        {
            return await _repo.GetResourceByIdAsync(id);
        }
    }
}
