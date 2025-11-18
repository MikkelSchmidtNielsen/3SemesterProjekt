using Application.ApplicationDto.Command;
using Common.ResultInterfaces;
using Domain.ModelsDto;

namespace Application.ServiceInterfaces.Command
{
    public interface IBookingCreateCommand
    {
        Task<IResult<BookingRequestResultDto>> CreateBookingAsync(BookingCreateRequestDto bookingCreateDto);
    }
}
