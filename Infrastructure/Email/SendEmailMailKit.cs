using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
using Common;
using Common.ResultInterfaces;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Email
{
	public class SendEmailMailKit : ISendEmail
	{
		private static readonly TimeOnly DefaultCheckIn = new(12, 0);
		private static readonly TimeOnly DefaultCheckOut = new(11, 0);
		private static readonly string DefaultSenderEmail = "noreply@foxtrox.dk";
		private static readonly string PasswordDefaultSender = "Foxtrox.dk";


		public IResult<SendEmailCommandDto> SendEmail(SendEmailCommandDto dto)
		{
			try
			{
				// Validate and Transform Email
				string emailInCorrectFormat = ValidateEmail(dto);

				// Creates the message
				string message = CreateMessage(dto);

				// Sends the email
				SendEmail(emailInCorrectFormat, dto.Subject, message);
				return Result<SendEmailCommandDto>.Success(dto);
			}
			catch (Exception ex)
			{
				return Result<SendEmailCommandDto>.Error(dto, ex);
			}

		}
		protected static string ValidateEmail(SendEmailCommandDto dto)
		{
			if (!MailboxAddress.TryParse(dto.Guest.Email, out MailboxAddress mail))
			{
				throw new Exception("Email was in wrong format");
			}
			if (!mail.Address.Contains("."))
			{
				throw new Exception("Email was in wrong format. Does not include an end address");
			}
			return mail.Address;
		}

		protected static string CreateMessage(SendEmailCommandDto dto)
		{
			if (dto.Guest.FirstName is null || string.IsNullOrWhiteSpace(dto.Guest.FirstName))
			{
				throw new Exception("Missing Information About the booking");
			}

			return $@"
					Hej {dto.Guest.FirstName},

					Tak for din booking!

					Her er detaljerne for din reservation:

					Ressource:
					- Navn: {dto.Resource.Name}
					- Type: {dto.Resource.Type}

					Periode:
					- Startdato: {dto.Booking.StartDate:dd-MM-yyyy}
					- Slutdato : {dto.Booking.EndDate:dd-MM-yyyy}

					Check-ind og check-ud:
					- Check-ind: {DefaultCheckIn:HH\\:mm}
					- Check-ud : {DefaultCheckOut:HH\\:mm}

					Pris:
					- Totalpris: {dto.Booking.TotalPrice:C}

					Vi glæder os til at byde dig velkommen!

					Venlig hilsen
					Dit Bookingteam
					";

		}

		protected void SendEmail(string receiverEmail, ISendEmail.EmailSubject subject, string message)
		{
			// MailKit format to send Email via MailKit NuggetPackage
			MimeMessage emailToBeSent = new MimeMessage();
			emailToBeSent.From.Add(new MailboxAddress("Foxtrox", DefaultSenderEmail));
			emailToBeSent.To.Add(new MailboxAddress("Kunde", receiverEmail));
			emailToBeSent.Subject = subject.ToString();

			var body = new TextPart("plain")
			{
				Text = message
			};

			emailToBeSent.Body = body;

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
