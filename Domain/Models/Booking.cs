using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Booking
    {
        public int Id { get; init; }
        public int GuestId { get; set; }
        public int ResourceId { get; set; }
        public string GuestName { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal TotalPrice { get; private set; }

        // Entity Framework
        public Guest Guest { get; set; }
        public Resource Resource { get; set; }

		public Booking(int guestId, int resourceId, string guestName, DateOnly startDate, DateOnly endDate, decimal totalPrice)
		{
			GuestId = guestId;
			ResourceId = resourceId;
			GuestName = guestName;
			StartDate = startDate;
			EndDate = endDate;
			TotalPrice = totalPrice;

            ValidateBookingInformation();
		}

		private void ValidateBookingInformation()
        {
            // Id
            if (Id <= 0)
            {
                throw new Exception();
            }
        }
    }
}
