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
        private readonly IResourceRepository _resourceRepository;
        private readonly IGuestRepository _guestRepository;

        public BookingCheckInQuery(IBookingRepository bookingRepository, IResourceRepository resourceRepository, IGuestRepository guestRepository)
        {
            _bookingRepository = bookingRepository;
            _resourceRepository = resourceRepository;
            _guestRepository = guestRepository;
        }

        public async Task<IResult<List<BookingMissingCheckInQueryDto>>> GetActiveBookingsWithMissingCheckInsAsync()
        {
            List<BookingMissingCheckInQueryDto> missingCheckIns = new List<BookingMissingCheckInQueryDto>();
            IResult<IEnumerable<Booking>> getMissingCheckIns = await _bookingRepository.GetActiveBookingsWithMissingCheckInsAsync();

            if (getMissingCheckIns.IsSucces() && getMissingCheckIns.GetSuccess().OriginalType.Any())
            {
                foreach(var booking in getMissingCheckIns.GetSuccess().OriginalType)
                {
                    IResult<Resource> resourceInfo = await _resourceRepository.GetResourceByIdAsync(booking.ResourceId);
                    IResult<Guest> guestInfo = await _guestRepository.GetGuestByIdAsync(booking.GuestId);
                    BookingMissingCheckInQueryDto missingCheckInInfo = new BookingMissingCheckInQueryDto
                    {
                        BookingId = booking.Id,
                        ResourceName = resourceInfo.GetSuccess().OriginalType.Name,
                        ResourceLocation = resourceInfo.GetSuccess().OriginalType.Location,
                        GuestName = $"{guestInfo.GetSuccess().OriginalType.FirstName} {guestInfo.GetSuccess().OriginalType.LastName}"
                    };
                    missingCheckIns.Add(missingCheckInInfo);
                }

                return Result<List<BookingMissingCheckInQueryDto>>.Success(missingCheckIns);
            }

            else
            {
                return Result<List<BookingMissingCheckInQueryDto>>.Error(missingCheckIns, new Exception("Der er ingen manglende indtjekninger."));
            }
        }
    }
}
