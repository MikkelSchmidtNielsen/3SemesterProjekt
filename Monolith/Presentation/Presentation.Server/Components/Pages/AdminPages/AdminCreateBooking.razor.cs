using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Application.ApplicationDto.Query.Responses;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Cmp;
using Radzen;
using Radzen.Blazor;
using System.Globalization;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class AdminCreateBooking
    {
        private decimal _tempTotalPrice;
        private string? _tempResource;

        string _bookingResult = "";

        IEnumerable<ReadAllResourceQueryResponseDto> _resources = Array.Empty<ReadAllResourceQueryResponseDto>();

        BookingModel _bookingModel = new BookingModel
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            Guest = new GuestCreateRequestDto()
        };

        ResourceFilterDto _filter = new ResourceFilterDto();

        protected override async Task OnInitializedAsync()
        {
            IResult<IEnumerable<ReadAllResourceQueryResponseDto>> result = await _resourceQuery.ReadAllResourcesAsync(_filter);

            if (result.IsSucces())
            {
                IEnumerable<ReadAllResourceQueryResponseDto> resources = result.GetSuccess().OriginalType;

                _resources = resources;
            }
            else
            {
                IResultError<IEnumerable<ReadAllResourceQueryResponseDto>> error = result.GetError();

                string message = error.Exception!.Message;

                await DialogService.Alert(message, "Error");
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
            foreach (ReadAllResourceQueryResponseDto resource in _resources)
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

            foreach (ReadAllResourceQueryResponseDto resource in _resources)
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

            _bookingModel = new BookingModel
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Guest = new GuestCreateRequestDto()
            };

            _tempResource = null;
            _tempTotalPrice = 0;
            await ShowDialog();
        }

    }

    internal class BookingModel
    {
        public int? ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public GuestCreateRequestDto Guest { get; set; }
    }
}
