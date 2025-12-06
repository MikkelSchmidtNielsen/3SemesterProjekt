using Domain.Models;

namespace Domain.ModelsDto
{
    public class CreateBookingFactoryDto
    {
        public int? Id { get; set; } // Booking id.
        public int ResourceId { get; set; }
        public int GuestId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Email { get; set; }
    }
}
