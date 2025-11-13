using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int guestId { get; set; }
        public int resourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public double TotalPrice { get; set; }

        // Entity Framework
        public Guest Guest { get; set; }
        public Resource Resource { get; set; }

        public Booking(DateOnly startTime, DateOnly endTime, double totalPrice, Guest guest, Resource resource)
        {
            StartDate = startTime;
            EndDate = endTime;
            TotalPrice = totalPrice;
            Guest = guest;
            Resource = resource;
        }
    }
}
