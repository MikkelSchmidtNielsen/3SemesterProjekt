using Domain.Models;

namespace Application.ApplicationDto.Command
{
    public class CreateBookingByGuestCommandDto
    {
        public string Email { get; set; }
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Guest? Guest { get; set; }
    }
}
