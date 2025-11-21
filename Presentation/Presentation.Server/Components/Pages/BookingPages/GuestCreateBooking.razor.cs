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

        string _guestBookingMessage = "";

        // 
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



            // Confirmation popup
            var selectedResource = _listOfResources.FirstOrDefault(r => r.Id == guestBookingModel.ResourceId);
            dto.ResourceId = guestBookingModel.ResourceId;

            var confirmed = await _dialogService.Confirm(
                @$"Vil du oprette denne booking: 
                   Email: {dto.Email},
                   Ressource: {selectedResource?.Name}
                   Start: {dto.StartDate}
                   Slut : {dto.EndDate}  
                   Pris : {dto.TotalPrice}",
                 
                  "Er du sikker?",

                new ConfirmOptions() { OkButtonText = "Ja", CancelButtonText = "Nej" }
            );

            if (confirmed == false)
            {
                return;
            }



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