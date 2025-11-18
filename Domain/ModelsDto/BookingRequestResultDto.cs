using Domain.Models;

namespace Domain.ModelsDto
{
    public class BookingRequestResultDto
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
