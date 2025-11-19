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
		public new string CreateMessageOrderConfirmation(SendEmailCommandDto dto)
		{
			return SendEmailMailKit.CreateMessageOrderConfirmation(dto);
		}

		/// <summary>
		/// Only used to test if it can send an email. DO NOT USE or TEST
		/// </summary>
		/// <param name="dto"></param>
		public new void SendEmail(SendEmailCommandDto dto)
		{
			base.SendEmail(dto);
		}
	}
}
