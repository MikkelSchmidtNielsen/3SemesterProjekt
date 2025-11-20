using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelsDto
{
    public class GuestCreateUserDomainDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Address { get; set; }
    }
}
