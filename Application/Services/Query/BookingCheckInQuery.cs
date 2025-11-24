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
    public class BookingCheckInQuery : IBookingCheckInQuery
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingCheckInQuery(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IResult<List<BookingMissingCheckInResponseDto>>> GetActiveBookingsWithMissingCheckInsAsync()
        {
            List<BookingMissingCheckInResponseDto> missingCheckIns = new List<BookingMissingCheckInResponseDto>();
            IResult<IEnumerable<Booking>> getMissingCheckIns = await _bookingRepository.GetActiveBookingsWithMissingCheckInsAsync();

            if (getMissingCheckIns.IsSucces() && getMissingCheckIns.GetSuccess().OriginalType.Any())
            {
                foreach(var booking in getMissingCheckIns.GetSuccess().OriginalType)
                {
                    BookingMissingCheckInResponseDto missingCheckInInfo = new BookingMissingCheckInResponseDto
                    {
                        BookingId = booking.Id,
                        ResourceName = booking.Resource.Name,
                        ResourceLocation = booking.Resource.Location,
                        BookingStartDate = booking.StartDate,
                        GuestName = $"{booking.Guest.FirstName} {booking.Guest.LastName}"
                    };
                    missingCheckIns.Add(missingCheckInInfo);
                }

                return Result<List<BookingMissingCheckInResponseDto>>.Success(missingCheckIns);
            }

            else
            {
                return Result<List<BookingMissingCheckInResponseDto>>.Error(missingCheckIns, new Exception("Der er ingen manglende indtjekninger."));
            }
        }
    }
}
