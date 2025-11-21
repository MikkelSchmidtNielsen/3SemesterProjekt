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
        private readonly IBookingRepository _repository;

        public BookingCheckInQuery(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<List<BookingMissingCheckInQueryDto>>> GetActiveBookingsWithMissingCheckInsAsync()
        {
            IResult<IEnumerable<Booking>> getMissingCheckIns = await _repository.GetActiveBookingsWithMissingCheckInsAsync();

            if (getMissingCheckIns.IsSucces() && getMissingCheckIns.GetSuccess().OriginalType.Any())
            {
                List<BookingMissingCheckInQueryDto> missingCheckIns = new List<BookingMissingCheckInQueryDto>();

                foreach(var booking in getMissingCheckIns.GetSuccess().OriginalType)
                {
                    BookingMissingCheckInQueryDto dto = Mapper.Map<BookingMissingCheckInQueryDto>(booking);
                    missingCheckIns.Add(dto);
                }
                return 
            }
            else
            {
                return Result<IEnumerable<Booking>>.Error(null, new Exception("Der er ingen bookinger med manglende indtjekninger :)"));
            }
        }
    }
}
