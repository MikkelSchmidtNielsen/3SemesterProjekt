using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Guest
    {
        public int Id { get; init; }
        public string FirstName { get; private set; }
        public string? LastName { get; private set; }
        public int PhoneNumber { get; private set; }
        public string? Email { get; private set; }
        public string? Country { get; private set; }
        public string? Language { get; private set; }
        public string? Address { get; private set; }

        // Entity Framework
        public List<Booking> Booking { get; }

        public Guest(string firstName, string? lastName, int phoneNumber, string? email, string? country, string? language, string? address)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Country = country;
            Language = language;
            Address = address;

            ValidateGuestInformation();
        }

		private void ValidateGuestInformation()
		{
            // First name must be provided
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                throw new ArgumentException("Fornavn skal tilføjes");
            }

            // Phone number must not be negative
            if (PhoneNumber < 0)
            {
                throw new ArgumentException("Telefonnummer kan ikke være negativt");
            }

            // Email is optional, if provided -> validate email: "@" and "." in correct order.
            if (!string.IsNullOrWhiteSpace(Email))
            {
                int atIndex = Email.IndexOf('@');
                int dotIndex = Email.LastIndexOf('.');

                if (atIndex <= 0 || dotIndex <= atIndex + 1 || dotIndex == Email.Length - 1)
                {
                    throw new ArgumentException("Fejl i @ og/eller .");
                }
            }
		}
	}
}
