using Application.ApplicationDto.Command;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InfrastructureInterfaces
{
	public interface ISendEmail
	{
		public enum EmailSubject { OrderConfirmation }
		IResult<SendEmailCommandDto> SendEmail(SendEmailCommandDto dto);
	}
}
