using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Domain.DomainInterfaces
{
	public interface IBookingFactory
	{
		IResult<Booking> Create(BookingCreateFactoryDto dto);
	}
}
