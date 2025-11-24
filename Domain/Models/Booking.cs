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
        public int GuestId { get; private set; }
        public int ResourceId { get; private set; }
        public string GuestName { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal TotalPrice { get; private set; }

        // Entity Framework
        public Guest Guest { get; set; }
        public Resource Resource { get; set; }
        private Booking() { }

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
            // StartDate/EndDate
            if (StartDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("StartDate is in the past.");
            }
            if (StartDate > EndDate)
            {
                throw new Exception("StartDate is after EndDate.");
            }
            if (StartDate == EndDate)
            {
                throw new Exception("StartDate and EndDate are on the same date.");
            }

            //TotalPrice
            if (TotalPrice < 0)
            {
                throw new Exception("TotalPrice is less than zero.");
            }
            if (DecimalPlaces(TotalPrice) > 2)
            {
                throw new Exception("There are more than 2 decimals.");
            }
        }
        /// <summary>
        /// Hjælpemetode: Returnerer antallet af decimaler et tal har.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int DecimalPlaces(decimal value)
        {
            value = Math.Abs(value);
            int[] bits = decimal.GetBits(value);
            byte scale = (byte)((bits[3] >> 16) & 0x7F);
            return scale;
        }
    }
}
