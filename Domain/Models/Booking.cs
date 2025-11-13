using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Booking
    {
        public int Id { get; private set; }
        public int guestId { get; set; }
        public int resourceId { get; set; }
        public string GuestName { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal TotalPrice { get; private set; }

        // Entity Framework
        public Guest Guest { get; set; }
        public Resource Resource { get; set; }

        public Booking(DateOnly startTime, DateOnly endTime, double totalPrice, Guest guest, Resource resource)
        {
            GuestName = guestName;
            StartDate = startDate;
            EndDate = endDate;
            StartDate = startTime;
            EndDate = endTime;
            TotalPrice = totalPrice;
            Guest = guest;
            Resource = resource;

            ValidateBookingInformation();
        }

        private void ValidateBookingInformation()
        {

        }
    }
}
