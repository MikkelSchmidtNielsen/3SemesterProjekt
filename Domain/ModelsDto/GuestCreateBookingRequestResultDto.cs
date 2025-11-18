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
        public Guest Guest { get; set; }
        public Booking Booking { get; set; }

    }
}
