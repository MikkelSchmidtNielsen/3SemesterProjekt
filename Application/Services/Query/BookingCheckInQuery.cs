using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
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

        public Task<IResult<IEnumerable<Booking>>> GetActiveBookingsWithMissingCheckIns()
        {
            throw new NotImplementedException();
        }
    }
}
