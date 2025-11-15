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

			string resourceName = "HytteA";
			string resourceType = "Hytte";
			Resource resource = Impression.Of<Resource>().
									With("ResourceName", resourceName).
									With("ResourceType", resourceType).
									WithDefaults().
									Create();

			string GuestName = "CustomerName";
			DateOnly startDate = DateOnly.FromDateTime(DateTime.Today);
			DateOnly endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
			Decimal totalPrice = 1200;
			Booking booking = Impression.Of<Booking>().
									With("Resource", resource).
									With("GuestName", GuestName).
									With("StartDate", startDate).
									With("EndDate", endDate).
									With("TotalPrice", totalPrice).
									WithDefaults().
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { ReceiverEmail = emailInWrongFormat, Subject = subject, Booking = booking };

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

			string resourceName = "HytteA";
			string resourceType = "Hytte";
			Resource resource = Impression.Of<Resource>().
									With("ResourceName", resourceName).
									With("ResourceType", resourceType).
									WithDefaults().
									Create();

			string GuestName = "CustomerName";
			DateOnly startDate = DateOnly.FromDateTime(DateTime.Today);
			DateOnly endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
			Decimal totalPrice = 1200;
			Booking booking = Impression.Of<Booking>().
									With("Resource", resource).
									With("GuestName", GuestName).
									With("StartDate", startDate).
									With("EndDate", endDate).
									With("TotalPrice", totalPrice).
									WithDefaults().
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { ReceiverEmail = email, Subject = subject, Booking = booking };

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
			string email = "noreply@danline.dk";
			ISendEmail.EmailSubject subject = ISendEmail.EmailSubject.OrderConfirmation;

			string resourceName = "HytteA";
			string resourceType = "Hytte";
			Resource resource = Impression.Of<Resource>().
									With("ResourceName", resourceName).
									With("ResourceType", resourceType).
									WithDefaults().
									Create();

			string GuestName = "CustomerName";
			DateOnly startDate = DateOnly.FromDateTime(DateTime.Today);
			DateOnly endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
			Decimal totalPrice = 1200;
			Booking booking = Impression.Of<Booking>().
									With("Resource", resource).
									With("GuestName", GuestName).
									With("StartDate", startDate).
									With("EndDate", endDate).
									With("TotalPrice", totalPrice).
									WithDefaults().
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { ReceiverEmail = email, Subject = subject, Booking = booking };

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
			string email = "noreply@danline.dk";
			ISendEmail.EmailSubject subject = ISendEmail.EmailSubject.OrderConfirmation;

			string resourceName = "HytteA";
			string resourceType = "Hytte";
			Resource resource = Impression.Of<Resource>().
									With("ResourceName", resourceName).
									With("ResourceType", resourceType).
									WithDefaults().
									Create();

			DateOnly startDate = DateOnly.FromDateTime(DateTime.Today);
			DateOnly endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
			Decimal totalPrice = 1200;
			Booking booking = Impression.Of<Booking>().
									With("Resource", resource).
									With("GuestName", null).
									With("StartDate", startDate).
									With("EndDate", endDate).
									With("TotalPrice", totalPrice).
									Create();

			SendEmailCommandDto emailDto = new SendEmailCommandDto() { ReceiverEmail = email, Subject = subject, Booking = booking };

			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act & Assert
			Assert.Throws<Exception>(() => mailKit.CreateMessage(emailDto));
		}
	}
}
