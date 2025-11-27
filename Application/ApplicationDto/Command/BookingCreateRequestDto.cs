using Domain.Models;

namespace Application.ApplicationDto.Command
{
    public class BookingCreateRequestDto
    {
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public GuestCreateRequestDto Guest { get; set; }
    }
}
