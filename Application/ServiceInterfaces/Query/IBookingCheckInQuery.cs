using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.ServiceInterfaces.Query
{
    public interface IBookingCheckInQuery
    {
        Task<IResult<IEnumerable<Booking>>> GetActiveBookingsWithMissingCheckIns();

    }
}
