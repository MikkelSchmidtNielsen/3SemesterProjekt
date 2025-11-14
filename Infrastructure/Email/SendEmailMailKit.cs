using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MailKit;
using MimeKit;

namespace Infrastructure.Email
{
	public class SendEmailMailKit : ISendEmail
	{
		public Task<IResult<SendEmailCommandDto>> SendEmail(SendEmailCommandDto dto)
		{
			throw new NotImplementedException();
		}
		protected void ValidateInformation(SendEmailCommandDto dto)
		{

			if (!MailboxAddress.TryParse(dto.ReceiverEmail, out MailboxAddress mail))
			{
				throw new Exception("Email was in wrong format");
			}
		}

		protected string CreateMessage(SendEmailCommandDto dto)
		{
			return "";
		}
	}
}
