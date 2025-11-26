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
        public Resource Resource { get; private set; } // private set for unit test.

        private Booking() { }

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
        protected Booking(int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice, Guest guest, Resource resource) // Constructor for unit test.
        {
            GuestId = guestId;
            ResourceId = resourceId;
            StartDate = startDate;
            EndDate = endDate;
            TotalPrice = totalPrice;
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
        /// Hjælpemetode: Returnerer antallet af decimaler et tal har.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetNumberOfDecimals(decimal input)
        {
            // Vi starter med at konvertere decimalen til en string: "converted" ja
            string converted = input.ToString(CultureInfo.InvariantCulture);

            // Check for at tallet skal indeholde decimaler
            if (converted.Contains("."))
            {
                // .Split() bruges til at dele "converted" ved komma-tallet, resultaterne gemmes i et string[]: "splitString"
                // Altså har vi nu et string[] med to indhold på to positioner:
                // 1. position ([0]): Alt før komma-tallet.
                // 2. position ([1]): Alt efter komma-tallet.
                string[] splitString = converted.Split('.');

                // .Lenght bruges til at tælle hvor mange chars der findes på "splitString"'s 2. position ([1]),
                // hvilket giver antallet af decimaler som gemmes som en int: "count".
                int count = splitString[1].Length;

                return count;
            }
            else return 0;
        }
    }
}
