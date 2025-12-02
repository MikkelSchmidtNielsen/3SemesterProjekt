using Application.ApplicationDto.Query.Responses;
using Application.InfrastructureInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Infrastructure.UnitTest;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Application.UnitTest.EmailSpecificationTest
{
	public class SendOrderConfirmationEmailTest
	{
		[Fact]
		public void CreateMessage_ShouldPass_WhenGivenDtoWithAllInformation()
		{
            // Arrange
            ReadResourceByIdQueryResponseDto resource = Impression.Of<ReadResourceByIdQueryResponseDto>().
									Randomize().
									Create();

			Guest guest = Impression.Of<Guest>().
									Randomize().
									Create();

			Booking booking = Impression.Of<Booking>().
									Randomize().
									Create();

			// Act
			string message = SendOrderConfirmationEmailTestClass.GenerateBody(booking, guest, resource);

			// Assert
			Assert.NotEmpty(message);
			Assert.False(string.IsNullOrWhiteSpace(message));
		}

		[Fact]
		public void CreateMessage_ShouldThrowException_WhenGivenDtoWithMissingCustomerName()
		{
            // Arrange
            ReadResourceByIdQueryResponseDto resource = Impression.Of<ReadResourceByIdQueryResponseDto>().
									Randomize().
									Create();

			Guest guest = Impression.Of<Guest>().
									With("FirstName", null).
									Randomize().
									Create();

			Booking booking = Impression.Of<Booking>().
									Randomize().
									Create();

			// Act & Assert
			Assert.Throws<Exception>(() => SendOrderConfirmationEmailTestClass.GenerateBody(booking, guest, resource));
		}
	}
}
