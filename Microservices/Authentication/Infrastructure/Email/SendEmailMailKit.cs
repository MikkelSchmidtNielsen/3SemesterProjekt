using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Application.InfrastructureInterfaces;

namespace Infrastructure.Email
{
    public class SendEmailMailKit : ISendEmail
    {
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
