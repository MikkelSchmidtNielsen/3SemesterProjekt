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
			try
			{
                Booking booking = new Booking(dto.GuestId, dto.ResourceId, dto.StartDate, dto.EndDate, dto.TotalPrice);
				return Result<Booking>.Success(booking);
            }
			catch (Exception ex)
			{
				return Result<Booking>.Error(null, ex);
			}
		}
	}
}
