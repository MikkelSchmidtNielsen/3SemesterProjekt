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
    public class GuestCreateBookingService : IGuestCreateBookingService
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestCreateUserService _guestCreateUser;

        public GuestCreateBookingService(IGuestRepository guestRepository, IResourceRepository resourceRepository, IBookingRepository bookingRepository, IGuestCreateUserService guestCreateUser)
        {
            _guestRepository = guestRepository;
            _resourceRepository = resourceRepository;
            _bookingRepository = bookingRepository;
            _guestCreateUser = guestCreateUser;
        }

        public async Task<IResult<Booking>> GuestCreateBookingAsync(GuestCreateBookingDto guestCreateBookingDto)
        {
            IResult<Guest> guest = await _guestRepository.GetGuestByIdAsync(guestCreateBookingDto.GuestId);

            IResult<Resource> resource = await _resourceRepository.GetResourceByIdAsync(guestCreateBookingDto.ResourceId);




            IResult<Booking> booking = await _bookingRepository.GuestCreateBookingAsync(Booking booking);
        }
    }
}
