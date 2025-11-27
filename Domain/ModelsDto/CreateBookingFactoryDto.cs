using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelsDto
{
    public class CreateBookingFactoryDto
    {
        public int? Id { get; set; } // Booking id.
        public int? ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Email { get; set; }
        public CreatedGuestDto Guest { get; set; } // Nullable because the guest might not exist yet.
        public Resource? Resource { get; set; }
    }
}
