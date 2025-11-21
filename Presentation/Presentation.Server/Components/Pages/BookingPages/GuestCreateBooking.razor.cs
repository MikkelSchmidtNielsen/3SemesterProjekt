using Application.ApplicationDto.Command;
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

                await _dialogService.Alert(message, "Error");
            }
        }

        // Create the booking 
        private async Task GuestCreateBookingAsync(GuestBookingModel guestBookingModel)
        {
            // Mapping
            GuestInputDto dto = Mapper.Map<GuestInputDto>(guestBookingModel);

            

            /*
            //// Calculate price
            decimal totalPrice = CalculateTotalPrice(dto);

            var confirmed = await _dialogService.Confirm(
                @$"<b>Vil du oprette denne booking?</b><br /><br />
                    Email: {dto.Email}<br />
                    Ressource: {selectedResource?.Name}<br />
                    Start: {dto.StartDate:dd/MM/yyyy}<br />
                    Slut: {dto.EndDate:dd/MM/yyyy}<br />
                    <p>Pris: {totalPrice} kr.<p/>",

                    "Er du sikker?",

                new ConfirmOptions() { OkButtonText = "Ja", CancelButtonText = "Nej" }
            );

            if (confirmed == false)
            {
                return;
            }
            */


            // Create the booking
            IResult<GuestInputDomainDto> result = await _guestCreateBookingService.HandleAsync(dto);

            if (result.IsSucces() == false)
            {
                Result<GuestInputDto>.Error(dto, result.GetError().Exception!);
                return;
            }
            GuestInputDomainDto domainDto = result.GetSuccess().OriginalType;

            // Message to guest:
            _guestBookingMessage = @$"Hej {domainDto.Guest.FirstName}
                                     Velkommen tilbage!
                                     Din booking er oprettet for: {domainDto.Resource.Name}
                                     Fra : {domainDto.StartDate}
                                     Til : {domainDto.EndDate}
                                     Pris: {domainDto.TotalPrice}";

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
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Guest? Guest { get; set; }
        public Resource Resource { get; set; }
    }
}