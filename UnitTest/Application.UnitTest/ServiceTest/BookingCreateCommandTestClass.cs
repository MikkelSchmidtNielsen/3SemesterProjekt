using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    internal class BookingCreateCommandTestClass : BookingCreateCommand
    {
		public BookingCreateCommandTestClass(IBookingRepository repository, IResourceIdQuery resourceIdQuery, IBookingFactory bookingFactory, IGuestCreateCommand guestCreateCommand, ISendEmail sendEmail) 
            : base(repository, resourceIdQuery, bookingFactory, guestCreateCommand, sendEmail)
		{
		}
        private BookingCreateCommandTestClass() : base() { }

		new public async Task<IResult<Guest>> CreateGuestAsync(BookingRequestResultDto dto, BookingCreateRequestDto input)
        {
            return await base.CreateGuestAsync(dto, input);
        }

        new public void AddPriceToDto(BookingRequestResultDto dto, Resource resource)
        {
            base.AddPriceToDto(dto, resource);
        }
    }
}
