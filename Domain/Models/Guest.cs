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
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Address { get; set; }

        // Entity Framework
        public List<Booking> Booking { get;}

        public Guest(int id, string firstName, string lastName, int phoneNumber, string email, string country, string language, string address)
        {
            Id = id;
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
