using Application.ApplicationDto.Query;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
    public class BookingCheckOutQuery : IBookingCheckOutQuery
    {
        private readonly IBookingRepository _repository;

        public BookingCheckOutQuery(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<List<BookingMissingCheckOutResponseDto>>> GetFinishedBookingsWithMissingCheckOutsAsync()
        {
            List<BookingMissingCheckOutResponseDto> missedCheckOuts = new List<BookingMissingCheckOutResponseDto>(); // Creates list for missing check outs
            IResult<IEnumerable<Booking>> getResultFromRepo = await _repository.GetFinishedBookingsWithMissingCheckOutsAsync();

            if (getResultFromRepo.IsSucces() && getResultFromRepo.GetSuccess().OriginalType.Any())
            {
                foreach (var booking in getResultFromRepo.GetSuccess().OriginalType) // Iterates through the list and converts the booking into a dto.
                {
                    BookingMissingCheckOutResponseDto missedCheckOutInfo = new BookingMissingCheckOutResponseDto
                    {
                        BookingId = booking.Id,
                        ResourceName = booking.Resource.Name,
                        ResourceLocation = booking.Resource.Location,
                        BookingEndDate = booking.EndDate,
                        GuestName = $"{booking.Guest.FirstName} {booking.Guest.LastName}"
                    };
                    missedCheckOuts.Add(missedCheckOutInfo);
                }
                return Result<List<BookingMissingCheckOutResponseDto>>.Success(missedCheckOuts);
            }
            else
            {
                return Result<List<BookingMissingCheckOutResponseDto>>.Error(missedCheckOuts, new Exception("Der er ingen manglende udtjekninger."));
            }
        }
    }
}