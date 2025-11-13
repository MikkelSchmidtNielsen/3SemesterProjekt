using Application.ApplicationDto.BookingDto;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.ServiceInterfaces.Command
{
    public interface IBookingCreateCommand
    {
        Task<IResult<Booking>> CreateBookingAsync(BookingCreateDto bookingCreateDto);
    }
}
