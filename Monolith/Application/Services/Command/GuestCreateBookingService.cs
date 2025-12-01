using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Application.Services.Command
{
    public class GuestCreateBookingService : ICreateBookingByGuestCommandHandler
    {
        private readonly IGuestRepository _guestRepository;
        //private readonly IResourceRepository _resourceRepository;
        private readonly IBookingFactory _bookingFactory;
        private readonly IBookingRepository _bookingRepository;

        public GuestCreateBookingService(IGuestRepository guestRepository, IBookingFactory bookingFactory, IBookingRepository bookingRepository)
        {
            _guestRepository = guestRepository;
            //_resourceRepository = resourceRepository;
            _bookingFactory = bookingFactory;
            _bookingRepository = bookingRepository;
        }

        public async Task<IResult<CreateBookingByGuestResponseDto>> HandleAsync(CreateBookingByGuestCommandDto inputDto)
        {
            CreateBookingFactoryDto domainDto = Mapper.Map<CreateBookingFactoryDto>(inputDto);

            // Check if the guest already has a user:
            IResult<Guest> guestUserRequest = await _guestRepository.ReadGuestByEmailAsync(domainDto.Email);

            if (guestUserRequest.IsSucces() == false)
            {
                CreateBookingByGuestResponseDto createdDto = Mapper.Map<CreateBookingByGuestResponseDto>(domainDto);

                return Result<CreateBookingByGuestResponseDto>.Error(createdDto, guestUserRequest.GetError().Exception!);
            }
            Guest guestResult = guestUserRequest.GetSuccess().OriginalType;
            
            // Apply the found guest to the domainDto
            domainDto.Guest = guestResult;

            // Get the resource
            //IResult<Resource> resourceRequest = await _resourceRepository.GetResourceByIdAsync(domainDto.Resource.Id);

            //if (resourceRequest.IsSucces() == false)
            //{
            //    CreateBookingByGuestResponseDto createdDto = Mapper.Map<CreateBookingByGuestResponseDto>(domainDto);
            //    return Result<CreateBookingByGuestResponseDto>.Error(createdDto, resourceRequest.GetError().Exception!);
            //}
            //Resource resourceResult = resourceRequest.GetSuccess().OriginalType;
            
            //// Apply the found resource & totalPrice to domainDto
            //domainDto.Resource = resourceResult;
            //domainDto.TotalPrice = CalculateTotalPrice(domainDto, resourceResult);

            // Create booking
            IResult<Booking> bookingCreateRequest = _bookingFactory.Create(domainDto);
            if (bookingCreateRequest.IsSucces() == false)
            {
                CreateBookingByGuestResponseDto createdDto = Mapper.Map<CreateBookingByGuestResponseDto>(domainDto);
                return Result<CreateBookingByGuestResponseDto>.Error(createdDto, bookingCreateRequest.GetError().Exception!);
            }
            Booking bookingCreateResult = bookingCreateRequest.GetSuccess().OriginalType;

            // Save the booking in DB:
            IResult<Booking> bookingSaveRequest = await _bookingRepository.CreateBookingAsync(bookingCreateResult);
            if (bookingSaveRequest.IsSucces() == false)
            {
                CreateBookingByGuestResponseDto createdDto = Mapper.Map<CreateBookingByGuestResponseDto>(domainDto);
                return Result<CreateBookingByGuestResponseDto>.Error(createdDto, bookingSaveRequest.GetError().Exception!);
            }

            // Create the DTO which is to be returned to the UI
            CreateBookingByGuestResponseDto returnToUIDto = new CreateBookingByGuestResponseDto
            {
                Guest = guestResult,
                StartDate = domainDto.StartDate,
                EndDate = domainDto.EndDate
            };

            return Result<CreateBookingByGuestResponseDto>.Success(returnToUIDto);
        }

        // Calculate TotalPrice for resource
        protected decimal CalculateTotalPrice(CreateBookingFactoryDto domainDto, Resource resource)
        {
            // Today + total days of staying
            int days = domainDto.EndDate.DayNumber - domainDto.StartDate.DayNumber + 1;
            return domainDto.TotalPrice = resource.BasePrice * days;
        }
    }
}

