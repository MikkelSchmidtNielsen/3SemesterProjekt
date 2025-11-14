using Application.ApplicationDto.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Infrastructure.UnitTest
{
	public class SendEmailMailKitUnitTest
	{
		[Theory]
		[InlineData("")]
		[InlineData("Danline@dk")]
		[InlineData("DanlineDk")]
		[InlineData("@Danline.dk")]
		[InlineData("  Danline.  dk")]
		public async Task ValidateInformation_ShouldThrowException_WhenGivenEmailInWrongFormat(string emailInWrongFormat)
		{
			// Arrange
			string subject = "UnitTestSubject";
			string body = "UnitTestBody";

			SendEmailCommandDto emailDto = new SendEmailCommandDto(emailInWrongFormat, subject, body);

			SendEmailMailKitUnitTestClass mailKit = new SendEmailMailKitUnitTestClass();

			// Act
			Assert.Throws<Exception>(() => mailKit.ValidateInformation(emailDto));
		}
	}
}
