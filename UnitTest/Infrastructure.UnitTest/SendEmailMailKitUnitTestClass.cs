using Application.ApplicationDto.Command;
using Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Infrastructure.UnitTest
{
	internal class SendEmailMailKitUnitTestClass : SendEmailMailKit
	{
		public new void ValidateInformation(SendEmailCommandDto dto)
		{
			base.ValidateInformation(dto);
		}
		public new string CreateMessage(SendEmailCommandDto dto)
		{
			return base.CreateMessage(dto);
		}
	}
}
