using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
using Application.InfrastructureInterfaces.SendEmailSpecifications;
using Common;
using Domain.Models;
using Moq;
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
			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act
			Assert.Throws<Exception>(() => mailKit.ValidateEmail(emailInWrongFormat));
		}

		[Fact]
		public void ValidateInformation_ShouldPass_WhenGivenCorrectInformation()
		{
			// Arrange
			string email = "noreply@danline.dk";

			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act
			string validatedEmail = mailKit.ValidateEmail(email);

			// Assert
			Assert.Equal(email, validatedEmail);
		}
	}
}
