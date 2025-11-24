using Application.ApplicationDto.Query;
using Application.ServiceInterfaces.Query;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
    public class BookingCheckOutQuery : IBookingCheckOutQuery
    {
        public Task<IResult<List<BookingMissingCheckOutResponseDto>>> GetFinishedBookingsWithMissingCheckOutsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
