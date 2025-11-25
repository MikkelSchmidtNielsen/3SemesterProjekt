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
        public bool isCheckedIn { get; set; }

        // Entity Framework
        public Guest Guest { get; private set; } // private set for unit test.
        public Resource Resource { get; private set; } // private set for unit test.

        private Booking() { }

        public Booking(int guestId, int resourceId, string guestName, DateOnly startDate, DateOnly endDate, decimal totalPrice)
		{
			GuestId = guestId;
			ResourceId = resourceId;
			GuestName = guestName;
			StartDate = startDate;
			EndDate = endDate;
			TotalPrice = totalPrice;
            isCheckedIn = false;

            ValidateBookingInformation();
		}

        protected Booking(Booking booking, Guest guest, Resource resource) // Constructor for unit test.
        {
            GuestId = booking.GuestId;
            ResourceId = booking.ResourceId;
            StartDate = booking.StartDate;
            EndDate = booking.EndDate;
            TotalPrice = booking.TotalPrice;
            Guest = guest;
            Resource = resource;
        }

		private void ValidateBookingInformation()
        {
            // Start date is future
            if (StartDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("Start datoen kan ikke være i fortiden");
            }
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

            // Start date is before end date
            if (StartDate > EndDate)
            {
                throw new ArgumentException("Slut datoen kan ikke være før start datoen");
            }

            // Start and end date is different
            if (StartDate == EndDate)
            {
                throw new ArgumentException("Start dato og slut dato må ikke være på samme dag");
            }

            // Total price is positive
            if (TotalPrice < 0)
            {
                throw new ArgumentException("Den totale pris kan ikke være negativ");
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
