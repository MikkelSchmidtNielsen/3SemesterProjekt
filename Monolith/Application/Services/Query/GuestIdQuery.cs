using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.Services.Query
{
    public class GuestIdQuery : IGuestIdQuery
    {
        private readonly IGuestRepository _repo;

        public GuestIdQuery(IGuestRepository repo)
        {
            _repo = repo;
        }

        public Task<IResult<Guest>> GetGuestByIdAsync(int id)
        {
            return _repo.GetGuestByIdAsync(id);
        }
    }
}
