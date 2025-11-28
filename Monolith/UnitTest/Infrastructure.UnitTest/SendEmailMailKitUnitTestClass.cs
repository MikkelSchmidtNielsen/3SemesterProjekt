using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
using Application.InfrastructureInterfaces.SendEmailSpecifications;
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
		public new string ValidateEmail(string email)
		{
			return SendEmailMailKit.ValidateEmail(email);
		}

		/// <summary>
		/// Only used to test if it can send an email. DO NOT USE or TEST
		/// </summary>
		/// <param name="spec"></param>
		public new void SendEmail(ISendEmailSpecification spec)
		{
			base.SendEmail(spec);
		}
	}
}
