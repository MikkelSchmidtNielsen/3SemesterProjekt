using Application.ApplicationDto.Query;
using Domain.Models;

namespace Application.ApplicationDto.Command
{
    public class CreateBookingByGuestResponseDto
    {
        public Guest Guest { get; set; }
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
