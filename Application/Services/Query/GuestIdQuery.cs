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
    public class GuestIdQuery : IGuestIdQuery
    {
        private readonly IGuestRepository _repo;

        public GuestIdQuery(IGuestRepository repo)
        {
            _repo = repo;
        }

        public Task<Guest> GetGuestByIdAsync(int id)
        {
            return _repo.GetGuestByIdAsync(id);
        }
    }
}
