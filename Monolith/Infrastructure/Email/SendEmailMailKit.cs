using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
using Common;
using Common.ResultInterfaces;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Application.InfrastructureInterfaces.SendEmailSpecifications;

namespace Infrastructure.Email
{
	public class SendEmailMailKit : ISendEmail
	{
		private static readonly string DefaultSenderEmail = "noreply@foxtrox.dk";
		private static readonly string PasswordDefaultSender = "Foxtrox.dk";


		public IResult<ISendEmailSpecification> SendEmail(ISendEmailSpecification emailSpecification)
		{
			try
			{
				// Validate and Transform Email
				string emailInCorrectFormat = ValidateEmail(emailSpecification.RecieverEmail);

				// Sends actual email
				SendEmail(emailInCorrectFormat, emailSpecification.Subject, emailSpecification.Body);

				return Result<ISendEmailSpecification>.Success(emailSpecification);
			}
			catch (Exception ex)
			{
				return Result<ISendEmailSpecification>.Error(emailSpecification, ex);
			}
		}

		protected static string ValidateEmail(string recieveremail)
		{
			if (!MailboxAddress.TryParse(recieveremail, out MailboxAddress mail))
			{
				throw new Exception("Email was in wrong format");
			}
			if (!mail.Address.Contains("."))
			{
				throw new Exception("Email was in wrong format. Does not include an end address");
			}
			return mail.Address;
		}

		protected void SendEmail(string receiverEmail, string subject, string body)
		{
			// MailKit format to send Email via MailKit NuggetPackage
			MimeMessage emailToBeSent = new MimeMessage();
			emailToBeSent.From.Add(new MailboxAddress("Foxtrox", DefaultSenderEmail));
			emailToBeSent.To.Add(new MailboxAddress("Kunde", receiverEmail));
			emailToBeSent.Subject = subject.ToString();

			var emailBody = new TextPart("plain")
			{
				Text = body
			};

			emailToBeSent.Body = emailBody;

			using (var client = new SmtpClient())
			{
				client.Timeout = 600;
				client.Connect("smtp.simply.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
				client.Authenticate(DefaultSenderEmail, PasswordDefaultSender);
				try
				{
					// sends email can throw IOException because of simply.com smtp server doesnt work well with MailKit, but it WILL send the email
					client.Send(emailToBeSent);
				}
				catch (IOException) { }

				client.Disconnect(true);
			}
		}
	}
}
