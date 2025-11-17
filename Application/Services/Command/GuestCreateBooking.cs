using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
    public class GuestCreateBooking : IGuestCreateBooking
    {
        private readonly IGuestRepository _guestRepository;


        public async Task<IResult<Booking>> GuestCreateBookingAsync(GuestCreateBookingDto guestCreateBookingDto)
        {
            IResult<Guest> guest = await _guestRepository
        }
    }
}
