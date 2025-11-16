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
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public string? Language { get; set; }
        public string? Address { get; set; }

        // Entity Framework
        public List<Booking> Booking { get;}

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

			// Email is optional, but if provided it must look like a valid email
			if (string.IsNullOrWhiteSpace(Email) == false && Email.Contains('@') == false)
			{
				throw new ArgumentException("Email er ikke korrekt");
			}
		}
	}
}
