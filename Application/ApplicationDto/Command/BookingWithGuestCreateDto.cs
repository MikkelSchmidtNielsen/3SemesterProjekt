namespace Application.ApplicationDto.Command
{
    public class BookingWithGuestCreateDto
    {
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public GuestCreateDto Guest { get; set; }
    }
}
