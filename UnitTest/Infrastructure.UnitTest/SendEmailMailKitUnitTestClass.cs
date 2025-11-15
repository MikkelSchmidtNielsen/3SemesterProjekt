using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
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
		public new string ValidateEmail(SendEmailCommandDto dto)
		{
			return SendEmailMailKit.ValidateEmail(dto);
		}
		public new string CreateMessage(SendEmailCommandDto dto)
		{
			return SendEmailMailKit.CreateMessage(dto);
		}
		public new void SendEmail(SendEmailCommandDto dto)
		{
			base.SendEmail(dto);
		}
	}
}
