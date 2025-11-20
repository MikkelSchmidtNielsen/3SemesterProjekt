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

        public async Task<IResult<GuestInputDomainDto>> HandleAsync(GuestInputDto inputDto)
        {
            // Create a new DTO to handle the possible different returns GuestCreateBookingRequestCommandDto (GCBRCD) request.
            // Also handles the issue with the GCBRCD residing in the Application layer, meaning;
            // in order for the factory, which resides in the Domain layer, to be able to use it, Domain would need to depend on Application... -
            // this is absolutely unacceptable! Thusly, GuestCreateBookingRequestResultDto (GCBRRD) is created solving this dependency-problem,
            // since GCBRRD resides in the Domain layer - beautiful!
            GuestInputDomainDto domainDto = Mapper.Map<GuestInputDomainDto>(inputDto);



            // Check if the guest already has a user:
            IResult<Guest> guestUserRequest = await _guestRepository.GetGuestByEmailAsync(domainDto.Email);
            // Skal man give metoden; GetGuestByEmailAsync GCBRCD eller domainDto med som parameter?

            if (guestUserRequest.IsSucces() == false)
            {
                return Result<GuestInputDomainDto>.Error(domainDto, guestUserRequest.GetError().Exception!);
            }
            var guestResult = guestUserRequest.GetSuccess().OriginalType;



            // Get the resource
            IResult<Resource> resourceRequest = await _resourceRepository.GetResourceByIdAsync(inputDto.ResourceId);

            if (resourceRequest.IsSucces() == false)
            {
                return Result<GuestInputDomainDto>.Error(domainDto, resourceRequest.GetError().Exception!);
            }
            var resourceResult = resourceRequest.GetSuccess().OriginalType;




            // Create the DTO which is to be returned to the UI
            var finalToBeReturnedDto = new GuestInputDomainDto
            {
                Guest = guestResult,
                Resource = resourceResult,
                StartDate = domainDto.StartDate,
                EndDate = domainDto.EndDate,
                TotalPrice = CalculateTotalPrice(domainDto, domainDto.Resource)
            };

            return Result<GuestInputDomainDto>.Success(finalToBeReturnedDto);
        }
        // Calculate TotalPrice for resource
        protected decimal CalculateTotalPrice(GuestInputDomainDto domainDto, Resource resource)
        {
            // Today + total days of staying
            int days = domainDto.EndDate.DayNumber - domainDto.StartDate.DayNumber + 1;
            return domainDto.TotalPrice = resource.BasePrice * days;
        }
    }
}

