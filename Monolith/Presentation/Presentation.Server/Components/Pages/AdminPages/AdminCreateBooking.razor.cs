using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Common;
using Common.ResultInterfaces;
using Domain.ModelsDto;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class AdminCreateBooking
    {
        private readonly ResourceFilterDto _filter = new ResourceFilterDto();
        private decimal _tempTotalPrice;
        private string? _tempResource;

        private string? _initializationError;
        private bool _hasShownErrorDialog;
        private string _bookingResult = "";

        IEnumerable<ReadResourceQueryResponseDto> _resources = Array.Empty<ReadResourceQueryResponseDto>();

        BookingModel _bookingModel = new BookingModel
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            Guest = new GuestCreateRequestDto()
        };

        protected override async Task OnInitializedAsync()
        {
            IResult<IEnumerable<ReadResourceQueryResponseDto>> result = await _resourceQuery.ReadAllResourcesAsync(_filter);

            if (result.IsSucces())
            {
                IEnumerable<ReadResourceQueryResponseDto> resources = result.GetSuccess().OriginalType;

                _resources = resources;
            }
            else
            {
                IResultError<IEnumerable<ReadResourceQueryResponseDto>> error = result.GetError();

                _initializationError = error.Exception!.Message;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!string.IsNullOrEmpty(_initializationError) && !_hasShownErrorDialog)
            {
                _hasShownErrorDialog = true;

                await DialogService.Alert(_initializationError, "Error");
            }
        }

        private void UpdateBookingPreview(int? resourceId, DateOnly startDate, DateOnly endDate)
        {
            // Since the method needs a non-nullable int, and we need resourceId to be nullable for the RadzenRequiredValidator Component, we convert resourceId to a non-nullable int
            int resourseIdToInt = resourceId.GetValueOrDefault();
            GetResourceNameById(resourseIdToInt);
            CalculateTempPrice(resourseIdToInt, startDate, endDate);
        }

        private void GetResourceNameById(int resourceId)
        {
            foreach (ReadResourceQueryResponseDto resource in _resources)
            {
                if (resource.Id == resourceId)
                {
                    _tempResource = resource.Name;
                    break;
                }
            }
        }
        private void CalculateTempPrice(int resourceId, DateOnly startDate, DateOnly endDate)
        {
            int days = endDate.DayNumber - startDate.DayNumber + 1;

            foreach (ReadResourceQueryResponseDto resource in _resources)
            {
                if (resource.Id == resourceId)
                {
                    _tempTotalPrice = resource.BasePrice * days;
                    break;
                }
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

            BookingCreateRequestDto dto = Mapper.Map<BookingCreateRequestDto>(model);

            IResult<BookingRequestResultDto> result = await _bookingCommand.CreateBookingAsync(dto);

            if (result.IsSucces())
            {
                IResultSuccess<BookingRequestResultDto> success = result.GetSuccess();

                _bookingResult = $"Bookingen er oprettet for {_resources.FirstOrDefault(resource => resource.Id == model.ResourceId)!.Name} med en total pris på {success.OriginalType.TotalPrice}";

                _bookingModel = new BookingModel
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                    Guest = new GuestCreateRequestDto()
                };
            }
            else if (result.IsError())
            {
                IResultError<BookingRequestResultDto> error = result.GetError();

                _bookingResult = $"{error.Exception!.Message}";
            }
            else if (result.IsConflict())
            {
                IResultConflict<BookingRequestResultDto> error = result.GetConflict();

                _bookingResult = $"{error.Exception!.Message}";
            }

            _tempResource = null;
            _tempTotalPrice = 0;
            await ShowDialog();
        }

        private void HandleShortCut(KeyboardEventArgs e)
        {
            if ((e.Key == "z" || e.Key == "Z") && e.CtrlKey)
            {
                _bookingModel.ResourceId = 1;
                _bookingModel.Guest.FirstName = "Jan Pan";
                _bookingModel.Guest.LastName = "Nees";
                _bookingModel.Guest.Address = "Japanvej 7";
                _bookingModel.Guest.PhoneNumber = 12345678;
            }
        }
    }

    internal class BookingModel
    {
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public GuestCreateRequestDto Guest { get; set; }
    }
}
