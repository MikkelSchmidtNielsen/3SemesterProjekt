using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InfrastructureInterfaces.SendEmailSpecifications
{
    public class SendOtpEmail : ISendEmailSpecification
    {
        public string ReceiverEmail { get; }

        public string Subject { get; }

        public string Body { get; }

        public SendOtpEmail(User user)
        {
            ReceiverEmail = user.Email!;

            Subject = "Engangskode";

            Body = GenerateBody(user);
        }
        protected static string GenerateBody(User user)
        {
            if (user.Otp is 0 || user.OtpExpiryTime.AddMinutes(30) > DateTime.UtcNow)
            {
                throw new Exception("Invalid One Time Password");
            }

            return $@"
					Hej,

                    Her er din engangskode:

                    {user.Otp}

                    Koden er gyldig i 30 minutter.

					Venlig hilsen
					Dit Bookingteam
					";
        }
    }
}
