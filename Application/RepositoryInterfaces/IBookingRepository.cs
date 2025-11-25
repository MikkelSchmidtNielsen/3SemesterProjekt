using Common;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
    public interface IBookingRepository
    {
        Task<IResult<Booking>> AdminCreateBookingAsync(Booking booking);
        Task<IResult<Booking>> GuestCreateBookingAsync(Booking booking);
        Task<IResult<IEnumerable<Booking>>> GetActiveBookingsWithMissingCheckInsAsync();
        Task<IResult<IEnumerable<Booking>>> GetFinishedBookingsWithMissingCheckOutsAsync();
    }
}
