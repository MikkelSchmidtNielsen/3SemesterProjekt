using Common.ResultInterfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
    public interface IBookingRepository
    {
        Task<IResult<Booking>> CreateBookingAsync(Booking booking);
    }
}
