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
    public class ResourceAllQuery : IResourceAllQuery
    {
        private readonly IResourceRepository _repo;

        public ResourceAllQuery(IResourceRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return _repo.GetAllResourcesAsync();
        }
    }
}
