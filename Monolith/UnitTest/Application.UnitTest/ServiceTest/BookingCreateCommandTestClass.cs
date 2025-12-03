using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Application.InfrastructureInterfaces;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    internal class BookingCreateCommandTestClass : BookingCreateCommand
    {
		public BookingCreateCommandTestClass(
            IBookingRepository repository, 
            IReadResourceByIdQuery resourceByIdQuery,
            IBookingFactory bookingFactory, 
            IReadGuestByEmailQuery guestByEmailQuery,
            IGuestCreateCommand guestCreateCommand, 
            ISendEmail sendEmail
        ) 
            : base
            (
                  repository, resourceByIdQuery, bookingFactory, guestByEmailQuery, guestCreateCommand, sendEmail
            )
		{
		}

        new public void AddPriceToDto(BookingRequestResultDto dto, ReadResourceByIdQueryResponseDto resource)
        {
            base.AddPriceToDto(dto, resource);
        }
    }
}
