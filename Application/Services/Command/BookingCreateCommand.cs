using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Query;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;

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

        public async Task<IResult<Booking>> CreateBookingAsync(BookingWithGuestCreateDto bookingCreateDto)
        {
            // Get resource by id for price calculation
            IResult<Resource> resourceResult = await _resourceIdQuery.GetResourceByIdAsync(bookingCreateDto.ResourceId);

            if (resourceResult.IsError())
            {
                IResultError<Resource> resourceError = resourceResult.GetError();

				// Returns an error because the ressource could not be found, so no booking could be created
				return Result<Booking>.Error(originalType: null!, exception: resourceError.Exception!);
			}

            Resource foundResource = resourceResult.GetSuccess().OriginalType;

            // Logic for calculating price
            int amountOfDays = bookingCreateDto.EndDate.DayNumber - bookingCreateDto.StartDate.DayNumber;
            decimal totalPrice = foundResource.BasePrice * amountOfDays;

            // Create guest
            IResult<Guest> guestResult = await _guestCreateCommand.CreateGuestAsync(bookingCreateDto.Guest);

            if (guestResult.IsError())
            {
				IResultError<Guest> guestError = guestResult.GetError();

				// Returns an error because the guest could not be created, so no booking exists
				return Result<Booking>.Error(originalType: null!, exception: guestError.Exception!);
			}

            Guest createdGuest = guestResult.GetSuccess().OriginalType;

            // Create booking
            IResult<Booking> result = _bookingFactory.Create(
                createdGuest.Id,
                bookingCreateDto.ResourceId,
                bookingCreateDto.StartDate,
                bookingCreateDto.EndDate,
                totalPrice
            );

            if (result.IsError())
            {
                IResultError<Booking> error = result.GetError();

                return Result<Booking>.Error(error.OriginalType, error.Exception!);
            }

            Booking createdBooking = result.GetSuccess().OriginalType;

            result = await _repository.CreateBookingAsync(createdBooking);

            return result;
        }
    }

}

