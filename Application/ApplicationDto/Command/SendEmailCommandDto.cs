using Application.InfrastructureInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.InfrastructureInterfaces.ISendEmail;

namespace Application.ApplicationDto.Command
{
	public record SendEmailCommandDto
	{
		public EmailSubject Subject { get; set; }
		public Booking Booking { get; set; }
		public Guest Guest { get; set; }
		public Resource Resource { get; set; }
	}
}
