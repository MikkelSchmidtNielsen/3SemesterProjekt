using Common;
using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryInterfaces
{
    public interface IBookingRepository
    {
        Task<IResult<Booking>> GuestCreateBookingAsync(Booking booking);
    }
}
