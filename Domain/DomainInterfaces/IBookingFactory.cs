using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Domain.DomainInterfaces
{
	public interface IBookingFactory
	{
		IResult<Booking> AdminCreate(BookingCreateFactoryDto dto);
		IResult<Booking> GuestCreate(GuestInputDomainDto dto);
	}
}
