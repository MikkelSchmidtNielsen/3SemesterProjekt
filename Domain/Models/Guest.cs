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
        public string LastName { get; private set; }
        public int PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Country { get; private set; }
        public string Language { get; private set; }
        public string Address { get; private set; }

        // Entity Framework
        public List<Booking> Booking { get;}

        private Guest() { }

        public Guest(string firstName, string lastName, int phoneNumber, string email, string country, string language, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Country = country;
            Language = language;
            Address = address;
        }

		private void ValidateGuestInformation()
		{

		}
	}
}
