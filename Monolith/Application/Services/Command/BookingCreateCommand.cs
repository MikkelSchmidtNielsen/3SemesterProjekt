using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query.Responses;
using Application.InfrastructureInterfaces;
using Application.InfrastructureInterfaces.SendEmailSpecifications;
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
    public class BookingCreateCommand : IBookingCreateCommand
    {
        private readonly IBookingRepository _repository;
        private readonly IReadResourceByIdQuery _readResourceByIdQuery;
        private readonly IBookingFactory _bookingFactory;
        private readonly IReadGuestByEmailQuery _readGuestByEmailQuery;
        private readonly IGuestCreateCommand _guestCreateCommand;
        private readonly ISendEmail _sendEmail;

        public BookingCreateCommand(IBookingRepository repository, IReadResourceByIdQuery readResourceByIdQuery, IBookingFactory bookingFactory, IReadGuestByEmailQuery readGuestByEmailQuery, IGuestCreateCommand guestCreateCommand, ISendEmail sendEmail)
        {
            _repository = repository;
            _readResourceByIdQuery = readResourceByIdQuery;
            _bookingFactory = bookingFactory;
            _readGuestByEmailQuery = readGuestByEmailQuery;
            _guestCreateCommand = guestCreateCommand;
            _sendEmail = sendEmail;
        }

        public async Task<IResult<BookingRequestResultDto>> CreateBookingAsync(BookingCreateRequestDto bookingCreateDto)
        {
            // Creates dto to handle different returns
            BookingRequestResultDto dto = Mapper.Map<BookingRequestResultDto>(bookingCreateDto);

            // Get resource by id for price calculation
            IResult<ReadResourceByIdQueryResponseDto> resourceQueryRequest = await _readResourceByIdQuery.ReadResourceByIdAsync(bookingCreateDto.ResourceId);

            if (resourceQueryRequest.IsError())
            {
                // Returns an error because the ressource could not be found, so no booking could be created
                return Result<BookingRequestResultDto>.Error(dto, resourceQueryRequest.GetError().Exception!);
            }

            AddPriceToDto(dto, resourceQueryRequest.GetSuccess().OriginalType);

            // Creates a Guest object to fill
            Guest? guest = null;

            // If email not added -> create the guest
            if (string.IsNullOrWhiteSpace(bookingCreateDto.Guest.Email))
            {
                // Create guest
                IResult<Guest> guestCreateRequest = await CreateGuestAsync(dto, bookingCreateDto);

                if (guestCreateRequest.IsSucces() == false)
                {
                    // Returns an error because the guest could not be created, so no booking exists
                    return Result<BookingRequestResultDto>.Error(dto, guestCreateRequest.GetError().Exception!);
                }

                // Get success
                guest = guestCreateRequest.GetSuccess().OriginalType;
            }
            else // Get the guest from database
            {
                // Check if guest exists
                IResult<Guest> guestQueryRequest = await _readGuestByEmailQuery.ReadGuestByEmailAsync(bookingCreateDto.Guest.Email!);

                if (guestQueryRequest.IsSucces() == false)
                {
                    // Returns an error because the guest could not be created, so no booking exists
                    return Result<BookingRequestResultDto>.Error(dto, guestQueryRequest.GetError().Exception!);
                }

                // Get success
                guest = guestQueryRequest.GetSuccess().OriginalType;

                // Add guest id to dto
                dto.GuestId = guest.Id;
            }

            // Create booking
            IResult<Booking> bookingCreateRequest = _bookingFactory.Create(Mapper.Map<CreateBookingFactoryDto>(dto));

            if (bookingCreateRequest.IsError())
            {
                return Result<BookingRequestResultDto>.Error(dto, bookingCreateRequest.GetError().Exception!);
            }

            // Creates booking in db
            IResult<Booking> repoCreateBookingRequest = await _repository.CreateBookingAsync(bookingCreateRequest.GetSuccess().OriginalType);

            if (repoCreateBookingRequest.IsSucces())
            {
                if (guest.Email is not null)
                {
                    Booking emailBooking = bookingCreateRequest.GetSuccess().OriginalType;
                    Guest emailGuest = guest;
                    ReadResourceByIdQueryResponseDto emailResource = resourceQueryRequest.GetSuccess().OriginalType;

                    SendOrderConfirmationEmail emailSpecification = new SendOrderConfirmationEmail(emailBooking, emailGuest, emailResource);

                    var emailResult = _sendEmail.SendEmail(emailSpecification);

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

        /// <summary>
        /// Calculates and adds price to dto
        /// </summary>
        protected void AddPriceToDto(BookingRequestResultDto dto, ReadResourceByIdQueryResponseDto resource)
        {
            // Today + total days of staying
            int days = dto.EndDate.DayNumber - dto.StartDate.DayNumber + 1;
            dto.TotalPrice = resource.BasePrice * days;
        }
    }
}

