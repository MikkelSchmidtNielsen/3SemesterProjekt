using Application.ApplicationDto.Command;
using Application.ServiceInterfaces.Query;
using Application.ApplicationDto.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Radzen;
using Domain.ModelsDto;

namespace Presentation.Server.Components.Pages.BookingPages
{
    public partial class GuestCreateBooking
    {
        IEnumerable<Resource> _listOfResources = Array.Empty<Resource>();

        GuestBookingModel _guestBookingModel = new GuestBookingModel
        {
            // Email
            // ResourceId
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            // TotalPrice
        };


        // Load resources
        protected override async Task OnInitializedAsync()
        {
            IResult<IEnumerable<Resource>> result = await _getAllResourcesService.GetAllResourcesAsync();

            if (result.IsSucces())
            {
                IEnumerable<Resource> resources = result.GetSuccess().OriginalType;

                _listOfResources = resources;
            }
            else
            {
                IResultError<IEnumerable<Resource>> error = result.GetError();

                string message = error.Exception!.Message;

                await DialogService.Alert(message, "Error");
            }
        }


        // Create the booking 
        private async Task<Result<GuestInputDto>> GuestCreateBookingAsync(GuestBookingModel guestBookingModel)
        {
            GuestInputDto dto = Mapper.Map<GuestInputDto>(guestBookingModel);

            IResult<GuestInputDomainDto> result = await _guestCreateBookingService.HandleAsync(dto);

            if (result.IsSucces() == false)
            {
                return Result<GuestInputDto>.Error(dto, result.GetError().Exception!);
            }
            Result<GuestInputDto> bookingResult = Result<GuestInputDto>.Success(dto);

            // Reset the page
            _guestBookingModel = new GuestBookingModel
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            };

            return bookingResult;
        }
    }
    internal class GuestBookingModel
    {
        public string Email { get; set; }
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}