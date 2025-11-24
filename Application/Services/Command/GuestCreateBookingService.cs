using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
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
        private readonly IBookingFactory _bookingFactory;
        private readonly IBookingRepository _bookingRepository;

        public GuestCreateBookingService(IGuestRepository guestRepository, IResourceRepository resourceRepository, IBookingFactory bookingFactory, IBookingRepository bookingRepository)
        {
            _guestRepository = guestRepository;
            _resourceRepository = resourceRepository;
            _bookingFactory = bookingFactory;
            _bookingRepository = bookingRepository;
        }

        public async Task<IResult<BookingCreatedDto>> HandleAsync(GuestInputDto inputDto)
        {
            GuestInputDomainDto domainDto = Mapper.Map<GuestInputDomainDto>(inputDto);

            // Check if the guest already has a user:
            IResult<Guest> guestUserRequest = await _guestRepository.GetGuestByEmailAsync(domainDto.Email);

            if (guestUserRequest.IsSucces() == false)
            {
                BookingCreatedDto createdDto = Mapper.Map<BookingCreatedDto>(domainDto);

                return Result<BookingCreatedDto>.Error(createdDto, guestUserRequest.GetError().Exception!);
            }
            Guest guestResult = guestUserRequest.GetSuccess().OriginalType;
            
            // Apply the found guest to the domainDto
            domainDto.Guest = guestResult;

            // Get the resource
            IResult<Resource> resourceRequest = await _resourceRepository.GetResourceByIdAsync(inputDto.ResourceId);

            if (resourceRequest.IsSucces() == false)
            {
                BookingCreatedDto createdDto = Mapper.Map<BookingCreatedDto>(domainDto);
                return Result<BookingCreatedDto>.Error(createdDto, resourceRequest.GetError().Exception!);
            }
            Resource resourceResult = resourceRequest.GetSuccess().OriginalType;
            
            // Apply the found resource & totalPrice to domainDto
            domainDto.Resource = resourceResult;
            domainDto.TotalPrice = CalculateTotalPrice(domainDto, resourceResult);

            // Create booking
            IResult<Booking> bookingCreateRequest = _bookingFactory.Create(domainDto);
            if (bookingCreateRequest.IsSucces() == false)
            {
                BookingCreatedDto createdDto = Mapper.Map<BookingCreatedDto>(domainDto);
                return Result<BookingCreatedDto>.Error(createdDto, guestUserRequest.GetError().Exception!);
            }
            Booking bookingCreateResult = bookingCreateRequest.GetSuccess().OriginalType;

            // Save the booking in DB:
            IResult<Booking> bookingSaveRequest = await _bookingRepository.GuestCreateBookingAsync(bookingCreateResult);
            if (bookingSaveRequest.IsSucces() == false)
            {
                BookingCreatedDto createdDto = Mapper.Map<BookingCreatedDto>(domainDto);
                return Result<BookingCreatedDto>.Error(createdDto, bookingSaveRequest.GetError().Exception!);
            }

            // Create the DTO which is to be returned to the UI
            BookingCreatedDto returnToUIDto = new BookingCreatedDto
            {
                Guest = guestResult,
                Resource = resourceResult,
                StartDate = domainDto.StartDate,
                EndDate = domainDto.EndDate,
                TotalPrice = CalculateTotalPrice(domainDto, resourceResult)
            };

            return Result<BookingCreatedDto>.Success(returnToUIDto);
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

