using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Application.Services.Command
{
    public class BookingCreateCommand : IBookingCreateCommand
    {
        private readonly IBookingRepository _repository;
        private readonly IResourceIdQuery _resourceIdQuery;
        private readonly IBookingFactory _bookingFactory;
        private readonly IGuestCreateCommand _guestCreateCommand;

        public BookingCreateCommand(IBookingRepository repository, IResourceIdQuery resourceIdQuery, IBookingFactory bookingFactory, IGuestCreateCommand guestCreateCommand)
        {
            _repository = repository;
            _resourceIdQuery = resourceIdQuery;
            _bookingFactory = bookingFactory;
            _guestCreateCommand = guestCreateCommand;
        }

        public async Task<IResult<CreatedBookingDto>> CreateBookingAsync(BookingCreateDto bookingCreateDto)
        {
            // Creates dto to handle different returns
            CreatedBookingDto dto = Mapper.Map<CreatedBookingDto>(bookingCreateDto);

            // Get resource by id for price calculation
            IResult<Resource> resourceResult = await _resourceIdQuery.GetResourceByIdAsync(bookingCreateDto.ResourceId);

            if (resourceResult.IsError())
            {
				// Returns an error because the ressource could not be found, so no booking could be created
				return Result<CreatedBookingDto>.Error(dto, resourceResult.GetError().Exception!);
            }

            AddPriceToDto(dto, resourceResult.GetSuccess().OriginalType);

            // Create guest
            IResult<Guest> guestResult = await CreateGuestAsync(dto, bookingCreateDto);

            if (guestResult.IsError())
            {
				// Returns an error because the guest could not be created, so no booking exists
				return Result<CreatedBookingDto>.Error(dto, guestResult.GetError().Exception!);
            }

            // Create booking
            IResult<Booking> bookingCreateResult = _bookingFactory.Create(dto);

            if (bookingCreateResult.IsError())
            {
                return Result<CreatedBookingDto>.Error(dto, bookingCreateResult.GetError().Exception!);
            }

            // Creates booking in db
            IResult<Booking> repoResult = await _repository.CreateBookingAsync(bookingCreateResult.GetSuccess().OriginalType);

            if (repoResult.IsError())
            {
                return Result<CreatedBookingDto>.Error(dto, repoResult.GetError().Exception!);
            }

            // Mapping the final booking
            CreatedBookingDto finalDto = Mapper.Map<CreatedBookingDto>(repoResult.GetSuccess().OriginalType);

            return Result<CreatedBookingDto>.Success(finalDto);
        }

        /// <summary>
        /// Creates guest and adds id to dto if succeeded
        /// </summary>
        protected async Task<IResult<Guest>> CreateGuestAsync(CreatedBookingDto dto, BookingCreateDto input)
        {
            IResult<Guest> guestResult = await _guestCreateCommand.CreateGuestAsync(input.Guest);

            if (guestResult.IsSucces())
            {
                dto.GuestId = guestResult.GetSuccess().OriginalType.Id;
            }

            return guestResult;
        }

        /// <summary>
        /// Calculates and adds price to dto
        /// </summary>
        protected void AddPriceToDto(CreatedBookingDto dto, Resource resource)
        {
            // Today + total days of staying
            int days = dto.EndDate.DayNumber - dto.StartDate.DayNumber + 1;
            dto.TotalPrice = resource.BasePrice * days;
        }
    }

}

