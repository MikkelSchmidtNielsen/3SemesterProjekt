using Application.InfrastructureInterfaces.SendEmailSpecifications;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.EmailSpecificationTest
{
	internal class SendOrderConfirmationEmailTestClass : SendOrderConfirmationEmail
	{
		// Not need for test
		public SendOrderConfirmationEmailTestClass(Booking booking, Guest guest, Resource resource) : base(booking, guest, resource)
		{
		}
		public static string GenerateBody(Booking booking, Guest guest, Resource resource)
		{
			return SendOrderConfirmationEmail.GenerateBody(booking, guest, resource);
		}
	}
}
