using Application.ApplicationDto.Command;
using Common.ExternalConfig;
using Common.ResultInterfaces;
using Domain.Models;
using Radzen;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class AdminCreateBooking
    {
        string _bookingResult = "";

        IEnumerable<Resource> _resources = Array.Empty<Resource>();

        BookingWithGuestCreateDto _bookingModel = new BookingWithGuestCreateDto
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            Guest = new GuestCreateDto()
        };

        protected override async Task OnInitializedAsync()
        {
            IResult<IEnumerable<Resource>> result = await _resourceQuery.GetAllResourcesAsync();

            if (result.IsSucces())
            {
                IEnumerable<Resource> resources = result.GetSuccess().OriginalType;

                _resources = resources;
            }
            else
            {
                IResultError<IEnumerable<Resource>> error = result.GetError();

                string message = error.Exception!.Message;

                await DialogService.Alert(message, "Error");
            }
		}

        private async Task CreateBookingAsync(BookingWithGuestCreateDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Guest.Address))
            {
                var confirmed = await DialogService.Confirm(
                    "Du har ikke udfyldt adressen. Er du sikker på at du vil fortsætte?",
                    "Er du sikker?",
                    new ConfirmOptions() { OkButtonText = "Ja", CancelButtonText = "Nej" }
                );

                if (confirmed == false)
                {
                    return;
                }
            }

            IResult<Booking> result = await _bookingCommand.CreateBookingAsync(_bookingModel);

            if (result.IsSucces())
            {
                IResultSuccess<Booking> success = result.GetSuccess();

                _bookingResult = $"Bookingen er oprettet for {success.OriginalType.Resource.Name} med en total pris på {success.OriginalType.TotalPrice}";
            }
            else if (result.IsError())
            {
                IResultError<Booking> error = result.GetError();

                _bookingResult = $"{error.Exception!.Message}";
            }
            else if (result.IsConflict())
            {
                IResultConflict<Booking> error = result.GetConflict();

                _bookingResult = $"{error.Exception!.Message}";
            }

            _bookingModel = new BookingWithGuestCreateDto
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Guest = new GuestCreateDto()
            };
        }
    }
}
