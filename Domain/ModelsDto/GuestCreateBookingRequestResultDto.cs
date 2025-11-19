using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelsDto
{
    public class GuestCreateBookingRequestResultDto
    {
        public string Email { get; set; }
        public Guest? Guest { get; set; } // Nullable because the guest might not exist yet.
        public Resource Resource { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
