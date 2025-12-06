using Common.ResultInterfaces;
using Domain.Models;

namespace Application.ServiceInterfaces.Query
{
    public interface IReadGuestByEmailQuery
    {
        Task<IResult<Guest>> ReadGuestByEmailAsync(string email);
    }
}
