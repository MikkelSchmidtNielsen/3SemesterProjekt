using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query.Responses;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Query;
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
        private readonly IReadResourceByIdQuery _readResourceByIdQuery;
        private readonly IBookingFactory _bookingFactory;
        private readonly IBookingRepository _bookingRepository;

        public GuestCreateBookingService(IGuestRepository guestRepository, IReadResourceByIdQuery readResourceByIdQuery, IBookingFactory bookingFactory, IBookingRepository bookingRepository)
        {
            _guestRepository = guestRepository;
            _readResourceByIdQuery = readResourceByIdQuery;
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
            domainDto.GuestId = guestResult.Id;

            // Get the resource
            IResult<ReadResourceByIdQueryResponseDto> resourceRequest = await _readResourceByIdQuery.ReadResourceByIdAsync(domainDto.ResourceId);

            if (resourceRequest.IsSucces() == false)
            {
                CreateBookingByGuestResponseDto createdDto = Mapper.Map<CreateBookingByGuestResponseDto>(domainDto);
                return Result<CreateBookingByGuestResponseDto>.Error(createdDto, resourceRequest.GetError().Exception!);
            }

            ReadResourceByIdQueryResponseDto resourceResult = resourceRequest.GetSuccess().OriginalType;

            // Apply the found resource & totalPrice to domainDto
            domainDto.ResourceId = resourceResult.Id;
            domainDto.TotalPrice = CalculateTotalPrice(domainDto, resourceResult);

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
                EndDate = domainDto.EndDate,
                TotalPrice = domainDto.TotalPrice
            };

            return Result<CreateBookingByGuestResponseDto>.Success(returnToUIDto);
        }

        // Calculate TotalPrice for resource
        protected decimal CalculateTotalPrice(CreateBookingFactoryDto domainDto, ReadResourceByIdQueryResponseDto resource)
        {
            // Today + total days of staying
            int days = domainDto.EndDate.DayNumber - domainDto.StartDate.DayNumber + 1;
            return domainDto.TotalPrice = resource.BasePrice * days;
        }
    }
}

