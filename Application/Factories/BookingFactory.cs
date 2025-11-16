using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;

namespace Application.Factories
{
    public class BookingFactory : IBookingFactory
    {
        public IResult<Booking> Create(int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice)
        {
            Booking booking = new Booking(guestId, resourceId, startDate, endDate, totalPrice);

            return Result<Booking>.Success(booking);
        }
    }
}
