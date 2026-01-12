using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Common;
using Common.ResultInterfaces;
using Microsoft.AspNetCore.Components.Web;
using Presentation.Server.Components.Pages.AdminPages;

namespace Presentation.Server.Components.Pages.BookingPages
{
    public partial class GuestCreateBooking
    {
        private readonly ResourceFilterDto _filter = new ResourceFilterDto();
        IEnumerable<ReadResourceQueryResponseDto> _listOfResources = Array.Empty<ReadResourceQueryResponseDto>();
        private ReadResourceQueryResponseDto? _selectedResource;

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
            IResult<IEnumerable<ReadResourceQueryResponseDto>> result = await _resourcesQuery.ReadAllResourcesAsync(_filter);

            if (result.IsSucces())
            {
                IEnumerable<ReadResourceQueryResponseDto> resources = result.GetSuccess().OriginalType;

                _listOfResources = resources;
            }
            else
            {
                IResultError<IEnumerable<ReadResourceQueryResponseDto>> error = result.GetError();

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

                // Reset the page
                _guestBookingModel = new GuestBookingModel
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                };

                await BookingConfirmationPopupAsync(bookingCreatedDto);
            }
        }
        
        private decimal CalculateTotalPrice(GuestBookingModel guestBookingModel, ReadResourceQueryResponseDto selectedResource)
        {
            // Today + total days of staying
            int days = guestBookingModel.EndDate.DayNumber - guestBookingModel.StartDate.DayNumber + 1;
            decimal totalPrice = 0;
            totalPrice += selectedResource.BasePrice * days;
            return totalPrice;
        }

        private void HandleShortCut(KeyboardEventArgs e)
        {
            if ((e.Key == "z" || e.Key == "Z") && e.CtrlKey)
            {
                _guestBookingModel.ResourceId = 1;
                _guestBookingModel.Email = "janpannees@japanese.jp";
            }
        }
    }
    internal class GuestBookingModel
    {
        public string Email { get; set; }
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}