using Application.ApplicationDto.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Radzen;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class AdminCreateBooking
    {
        string _bookingResult = "";

        IEnumerable<Resource> _resources = Array.Empty<Resource>();

        BookingModel _bookingModel = new BookingModel
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

        private async Task CreateBookingAsync(BookingModel model)
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

            BookingCreateDto dto = Mapper.Map<BookingCreateDto>(model);

            IResult<CreatedBookingDto> result = await _bookingCommand.CreateBookingAsync(dto);

            if (result.IsSucces())
            {
                IResultSuccess<CreatedBookingDto> success = result.GetSuccess();

                _bookingResult = $"Bookingen er oprettet for {_resources.FirstOrDefault(resource => resource.Id == model.ResourceId)!.Name} med en total pris på {success.OriginalType.TotalPrice}";
            }
            else if (result.IsError())
            {
                IResultError<CreatedBookingDto> error = result.GetError();

                _bookingResult = $"{error.Exception!.Message}";
            }
            else if (result.IsConflict())
            {
                IResultConflict<CreatedBookingDto> error = result.GetConflict();

                _bookingResult = $"{error.Exception!.Message}";
            }

            _bookingModel = new BookingModel
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Guest = new GuestCreateDto()
            };
        }
    }

    internal class BookingModel
    {
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public GuestCreateDto Guest { get; set; }
    }
}
