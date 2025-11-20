using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces;
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
        private readonly ISendEmail _sendEmail;

		public BookingCreateCommand(IBookingRepository repository, IResourceIdQuery resourceIdQuery, IBookingFactory bookingFactory, IGuestCreateCommand guestCreateCommand, ISendEmail sendEmail)
		{
			_repository = repository;
			_resourceIdQuery = resourceIdQuery;
			_bookingFactory = bookingFactory;
			_guestCreateCommand = guestCreateCommand;
			_sendEmail = sendEmail;
		}

		public async Task<IResult<BookingRequestResultDto>> CreateBookingAsync(BookingCreateRequestDto bookingCreateDto)
        {
            // Creates dto to handle different returns
            BookingRequestResultDto dto = Mapper.Map<BookingRequestResultDto>(bookingCreateDto);

            // Get resource by id for price calculation
            IResult<Resource> resourceQueryRequest = await _resourceIdQuery.GetResourceByIdAsync(bookingCreateDto.ResourceId);

            if (resourceQueryRequest.IsError())
            {
				// Returns an error because the ressource could not be found, so no booking could be created
				return Result<BookingRequestResultDto>.Error(dto, resourceQueryRequest.GetError().Exception!);
            }

            AddPriceToDto(dto, resourceQueryRequest.GetSuccess().OriginalType);

            // Create guest
            IResult<Guest> guestCreateRequest = await CreateGuestAsync(dto, bookingCreateDto);

            if (guestCreateRequest.IsError())
            {
				// Returns an error because the guest could not be created, so no booking exists
				return Result<BookingRequestResultDto>.Error(dto, guestCreateRequest.GetError().Exception!);
            }

            // Create booking
            IResult<Booking> bookingCreateRequest = _bookingFactory.Create(Mapper.Map<BookingCreateFactoryDto>(dto));

            if (bookingCreateRequest.IsError())
            {
                return Result<BookingRequestResultDto>.Error(dto, bookingCreateRequest.GetError().Exception!);
            }

            // Creates booking in db
            IResult<Booking> repoCreateBookingRequest = await _repository.CreateBookingAsync(bookingCreateRequest.GetSuccess().OriginalType);

            if (repoCreateBookingRequest.IsSucces())
            {
                if (guestCreateRequest.GetSuccess().OriginalType.Email is not null)
                {
                    Booking emailBooking = bookingCreateRequest.GetSuccess().OriginalType;
                    Guest emailGuest = guestCreateRequest.GetSuccess().OriginalType;
                    Resource emailResource = resourceQueryRequest.GetSuccess().OriginalType;

                    var emailResult = SendEmail(emailBooking, emailGuest, emailResource);

					if (!emailResult.IsSucces())
					{
						return Result<BookingRequestResultDto>.Error(dto, repoCreateBookingRequest.GetError().Exception!);
					}
				}

                // Mapping the final booking
                BookingRequestResultDto finalDto = Mapper.Map<BookingRequestResultDto>(repoCreateBookingRequest.GetSuccess().OriginalType);

                return Result<BookingRequestResultDto>.Success(finalDto);
            }
            else
            {
                return Result<BookingRequestResultDto>.Error(dto, repoCreateBookingRequest.GetError().Exception!);
            }
            // NEEDS A CONFLICT RETURN
        }

        /// <summary>
        /// Creates guest and adds id to dto if succeeded
        /// </summary>
        protected async Task<IResult<Guest>> CreateGuestAsync(BookingRequestResultDto dto, BookingCreateRequestDto input)
        {
            IResult<Guest> guestResult = await _guestCreateCommand.CreateGuestAsync(input.Guest);

            if (guestResult.IsSucces())
            {
                dto.GuestId = guestResult.GetSuccess().OriginalType.Id;
            }

            return guestResult;
        }
        private IResult<SendEmailCommandDto> SendEmail(Booking booking, Guest guest, Resource resource)
        {
			SendEmailCommandDto emailDto = new SendEmailCommandDto()
			{
				Subject = ISendEmail.EmailSubject.OrderConfirmation,
				Booking = booking,
				Guest = guest,
				Resource = resource
			};

			var emailResult = _sendEmail.SendEmail(emailDto);

            return emailResult;
		}

        /// <summary>
        /// Calculates and adds price to dto
        /// </summary>
        protected void AddPriceToDto(BookingRequestResultDto dto, Resource resource)
        {
            // Today + total days of staying
            int days = dto.EndDate.DayNumber - dto.StartDate.DayNumber + 1;
            dto.TotalPrice = resource.BasePrice * days;
        }
    }
}

