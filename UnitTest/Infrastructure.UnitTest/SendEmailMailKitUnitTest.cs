using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
using Common;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Infrastructure.UnitTest
{
	public class SendEmailMailKitUnitTest
	{
		[Theory]
		[InlineData("")]
		[InlineData("Danline@dk")]
		[InlineData("DanlineDk")]
		[InlineData("@Danline.dk")]
		public void ValidateInformation_ShouldThrowException_WhenGivenEmailInWrongFormat(string emailInWrongFormat)
		{
			// Arrange
			ISendEmail.EmailSubject subject = ISendEmail.EmailSubject.OrderConfirmation;

			Resource resource = Impression.Of<Resource>().
									WithDefaults().
									Create();

			Guest guest = Impression.Of<Guest>().
                                    With("Email", emailInWrongFormat).	
									WithDefaults().
									Create();

			Booking booking = Impression.Of<Booking>().
									WithDefaults().
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { Subject = subject, Booking = booking, Guest = guest, Resource = resource };

			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act
			Assert.Throws<Exception>(() => mailKit.ValidateEmail(emailDto));
		}

		[Fact]
		public void ValidateInformation_ShouldPass_WhenGivenCorrectInformation()
		{
			// Arrange
			string email = "noreply@danline.dk";
			ISendEmail.EmailSubject subject = ISendEmail.EmailSubject.OrderConfirmation;

			Resource resource = Impression.Of<Resource>().
									Randomize().
									Create();

			Guest guest = Impression.Of<Guest>().
									With("Email", email).
									Randomize().
									Create();

			Booking booking = Impression.Of<Booking>().
									Randomize().
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { Subject = subject, Booking = booking, Guest = guest, Resource = resource };

			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act
			string validatedEmail = mailKit.ValidateEmail(emailDto);

			// Assert
			Assert.Equal(email, validatedEmail);
		}

		[Fact]
		public void CreateMessage_ShouldPass_WhenGivenDtoWithAllInformation()
		{
			// Arrange
			ISendEmail.EmailSubject subject = ISendEmail.EmailSubject.OrderConfirmation;

			Resource resource = Impression.Of<Resource>().
									Randomize().
									Create();

			Guest guest = Impression.Of<Guest>().
									Randomize().
									Create();

			Booking booking = Impression.Of<Booking>().
									Randomize().
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { Subject = subject, Booking = booking, Guest = guest, Resource = resource };

			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act
			string message = mailKit.CreateMessage(emailDto);

			// Assert
			Assert.NotEmpty(message);
			Assert.False(string.IsNullOrWhiteSpace(message));
		}

		[Fact]
		public void CreateMessage_ShouldThrowException_WhenGivenDtoWithMissingCustomerName()
		{
			// Arrange
			ISendEmail.EmailSubject subject = ISendEmail.EmailSubject.OrderConfirmation;

			Resource resource = Impression.Of<Resource>().
									Randomize().
									Create();

			Guest guest = Impression.Of<Guest>().
									With("FirstName", null).
									Randomize().
									Create();

			Booking booking = Impression.Of<Booking>().
									Randomize().
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { Subject = subject, Booking = booking, Guest = guest, Resource = resource };

			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act & Assert
			Assert.Throws<Exception>(() => mailKit.CreateMessage(emailDto));
		}
	}
}
