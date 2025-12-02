using Application.ApplicationDto.Query.Responses;
using Application.InfrastructureInterfaces.SendEmailSpecifications;
using Domain.Models;

namespace UnitTest.Application.UnitTest.EmailSpecificationTest
{
	internal class SendOrderConfirmationEmailTestClass : SendOrderConfirmationEmail
	{
		// Not need for test
		public SendOrderConfirmationEmailTestClass(Booking booking, Guest guest, ReadResourceByIdQueryResponseDto resource) : base(booking, guest, resource)
		{
		}
		public static string GenerateBody(Booking booking, Guest guest, ReadResourceByIdQueryResponseDto resource)
		{
			return SendOrderConfirmationEmail.GenerateBody(booking, guest, resource);
		}
	}
}
