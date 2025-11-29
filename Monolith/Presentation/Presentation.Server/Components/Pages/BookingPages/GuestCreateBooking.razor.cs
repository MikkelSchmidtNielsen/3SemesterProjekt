using Application.ApplicationDto.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Microsoft.VisualBasic;
using Radzen;
using Radzen.Blazor;

namespace Presentation.Server.Components.Pages.BookingPages
{
    public partial class GuestCreateBooking
    {
        IEnumerable<Resource> _listOfResources = Array.Empty<Resource>();

        string _guestBookingMessage = "";

        GuestBookingModel _guestBookingModel = new GuestBookingModel
        {
            // Email needs no initialization.
            // ResourceId needs no initialization.
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            // TotalPrice needs no initialization.
        };

        // Load resources
        protected override async Task OnInitializedAsync()
        {
            IResult<IEnumerable<Resource>> result = await _getAllResourcesService.HandleAsync();

            if (result.IsSucces())
            {
                IEnumerable<Resource> resources = result.GetSuccess().OriginalType;

                _listOfResources = resources;
            }
            else
            {
                IResultError<IEnumerable<Resource>> error = result.GetError();

                string message = error.Exception!.Message;

                await _dialogService.Alert(message, "Error");
            }
        }

        // Create the booking 
        private async Task GuestCreateBookingAsync(GuestBookingModel guestBookingModel)
        {
            // Mapping
            CreateBookingByGuestCommandDto dto = Mapper.Map<CreateBookingByGuestCommandDto>(guestBookingModel);

            // Create the booking
            IResult<CreateBookingByGuestResponseDto> result = await _guestCreateBookingService.HandleAsync(dto);

            if (result.IsSucces() == false)
            {
                await BookingErrorPopupAsync(result.GetError().Exception!.Message);
            }
            else
            {
                CreateBookingByGuestResponseDto bookingCreatedDto = result.GetSuccess().OriginalType;

                await BookingConfirmationPopupAsync(bookingCreatedDto);
            }
            // Reset the page
            _guestBookingModel = new GuestBookingModel
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            };
        }
        
        private decimal CalculateTotalPrice(GuestBookingModel guestBookingModel)
        {
            // Today + total days of staying
            int days = guestBookingModel.EndDate.DayNumber - guestBookingModel.StartDate.DayNumber + 1;
            decimal totalPrice = 0;
            totalPrice += guestBookingModel.Resource.BasePrice * days;
            return totalPrice;
        }
    }
    internal class GuestBookingModel
    {
        public string Email { get; set; }
        public int? ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Resource Resource { get; set; }
    }
}