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

        // Entity Framework
        public Guest Guest { get; }
        public Resource Resource { get; }

        public Booking(int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice)
		{
			GuestId = guestId;
			ResourceId = resourceId;
			StartDate = startDate;
			EndDate = endDate;
			TotalPrice = totalPrice;

            ValidateBookingInformation();
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
        }
    }
}
