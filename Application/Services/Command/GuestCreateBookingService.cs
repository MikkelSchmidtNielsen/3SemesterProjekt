using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
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

        public async Task<IResult<GuestCreateBookingRequestResultDto>> GuestCreateBookingAsync(GuestCreateBookingRequestDto guestCreateBookingRequestCommandDto)
        {
            // Create a new DTO to handle the returns guestCreateBookingRequestCommandDto request
            GuestCreateBookingRequestResultDto domainDto = Mapper.Map<GuestCreateBookingRequestResultDto>(guestCreateBookingRequestCommandDto);

            IResult<Guest> guestUserRequest = await _guestRepository.GetGuestByEmailAsync(guestCreateBookingRequestCommandDto.Email);



            if (guestUserRequest.IsSucces() == false)
            {
                return Result<GuestCreateBookingRequestResultDto>.Error(domainDto, guestUserRequest.GetError().Exception!);
            }
            



            IResult<Resource> resource = await _resourceRepository.GetResourceByIdAsync(guestCreateBookingRequestCommandDto.ResourceId);




            IResult<Booking> booking = await _bookingRepository.GuestCreateBookingAsync(Booking booking);
        }
    }
}
