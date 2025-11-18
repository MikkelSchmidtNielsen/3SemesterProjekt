using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Application.Factories
{
	public class BookingFactory : IBookingFactory
	{
        public IResult<Booking> Create(BookingCreateFactoryDto dto)
		{
            Booking booking = new Booking(dto.GuestId, dto.ResourceId, dto.StartDate, dto.EndDate, dto.TotalPrice);

            return Result<Booking>.Success(booking);
		}
	}
}
