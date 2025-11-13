using Application.ApplicationDto.BookingDto;
using Application.ServiceInterfaces;

namespace Presentation.Server.Components.Pages
{
    public partial class Booking
    {
        BookingCreateDto _bookingModel = new BookingCreateDto
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };

        private async Task CreateBookingAsync(BookingCreateDto model)
        {
            await _bookingService.CreateBookingAsync(_bookingModel);
        }
    }
}
