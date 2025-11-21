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

        public async Task<IResult<IEnumerable<Booking>>> GetActiveBookingsWithMissingCheckInsAsync()
        {
            //var result = await _repository.GetActiveBookingsWithMissingCheckInsAsync();

            //if(result.IsSucces() && result.GetSuccess().OriginalType.Any())
            //{

            //    foreach
            //}
            //else
            //{
            //    return Result<IEnumerable<Booking>>.Error(null, new Exception("Der er ingen bookinger med manglende indtjekninger :)"));
            //}
            throw new NotImplementedException();
        }
    }
}
