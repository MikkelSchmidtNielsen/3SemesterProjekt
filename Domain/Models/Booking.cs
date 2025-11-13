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
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal TotalPrice { get; private set; }

        public Booking(DateOnly startDate, DateOnly endDate, decimal totalPrice)
        {
            StartDate = startDate;
            EndDate = endDate;
            TotalPrice = totalPrice;

            ValidateBookingInformation();
        }

        private void ValidateBookingInformation()
        {
            // Start dato er i nutid
            if (StartDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("Start datoen kan ikke være i fortiden");
            }

            // Start dato er sat før slut dato
            if (StartDate > EndDate)
            {
                throw new ArgumentException("Slut datoen kan ikke være før start datoen");
            }

            // Start og slut dato er forskellige
            if (StartDate == EndDate)
            {
                throw new ArgumentException("Start dato og slut dato må ikke være på samme dag");
            }

            // Total pris er positiv
            if (TotalPrice < 0)
            {
                throw new ArgumentException("Den totale pris kan ikke være negativ");
            }
        }
    }
}
