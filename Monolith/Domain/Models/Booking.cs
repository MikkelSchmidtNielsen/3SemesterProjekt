using System.Globalization;

namespace Domain.Models
{
    public class Booking
    {
        public int Id { get; init; }
        public int GuestId { get; private set; }
        public int ResourceId { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal TotalPrice { get; private set; }
        public bool isCheckedIn { get; private set; }
        public bool isCheckedOut { get; private set; }

        // Entity Framework
        public Guest Guest { get; private set; } // private set for unit test.

        protected Booking() { }

        public Booking(int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice)
		{
			GuestId = guestId;
			ResourceId = resourceId;
			StartDate = startDate;
			EndDate = endDate;
			TotalPrice = totalPrice;
            isCheckedIn = false;
            isCheckedOut = false;

            ValidateBookingInformation();
		}
        protected Booking(int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice, Guest guest) // Constructor for unit test.
        {
            GuestId = guestId;
            ResourceId = resourceId;
            StartDate = startDate;
            EndDate = endDate;
            TotalPrice = totalPrice;
            Guest = guest;
        }


        private void ValidateBookingInformation()
        {
            // Start date is future
            if (StartDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("Start datoen kan ikke være i fortiden");
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
            // Number of decimals
            if (GetNumberOfDecimals(TotalPrice) > 2)
            {
                throw new ArgumentException("Der er mere end 2 decimaler i tallet");
            }
        }
        /// <summary>
        /// Returns a number's amount of decimals
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static int GetNumberOfDecimals(decimal TotalPrice)
        {
            // Convert the decimal to a string, ignoring CultureInfo so that it works with both DOT and COMMAS.
            string converted = TotalPrice.ToString(CultureInfo.InvariantCulture);

            // Check that the number indeed contains punctuation
            if (converted.Contains("."))
            {
                // Split() is used to create a string[] when a punctuation mark is encountered, with everything before the mark
                // in position[0] and everything after the mark in position [1].
                string[] splitString = converted.Split('.');

                // .Lenght is used to count how many chars is in position[1] of the array,
                // resulting in the number of decimals a number has
                int count = splitString[1].Length;

                return count;
            }
            // Return 0 if the number doesn't have a punctuation mark
            else return 0;
        }
    }
}
