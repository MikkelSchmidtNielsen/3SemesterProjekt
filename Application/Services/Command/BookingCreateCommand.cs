using Application.ApplicationDto.BookingDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.Services.Command
{
    public class BookingCreateCommand : IBookingCreateCommand
    {
        private readonly IBookingRepository _repository;

        public BookingCreateCommand(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<Booking>> CreateBookingAsync(BookingCreateDto bookingCreateDto)
        {
            decimal price = 200;

            int amountOfDays = bookingCreateDto.EndDate.DayNumber - bookingCreateDto.StartDate.DayNumber;
            decimal totalPrice = price * amountOfDays;

            Booking booking = new Booking
            (
                bookingCreateDto.GuestName,
                bookingCreateDto.StartDate,
                bookingCreateDto.EndDate,
                totalPrice
            );

            IResult<Booking> result = await _repository.CreateBookingAsync(booking);

            return result;
        }
    }
}
