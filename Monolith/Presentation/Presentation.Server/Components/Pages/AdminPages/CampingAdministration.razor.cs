using Application.ApplicationDto.Query;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class CampingAdministration
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        bool checkInActive = true;
        bool checkOutActive = false;
        bool checkInButtonDisabled = true;
        bool checkOutButtonDisabled = false;

        IEnumerable<BookingMissingCheckInResponseDto> _missingCheckIns = new List<BookingMissingCheckInResponseDto>();
        IEnumerable<BookingMissingCheckOutResponseDto> _missingCheckOuts = new List<BookingMissingCheckOutResponseDto>();

        private void checkInSelected()
        {
            checkInActive = true;
            checkOutActive = false;
            checkInButtonDisabled = true;
            checkOutButtonDisabled = false;
            StateHasChanged();
        }
        private void checkOutSelected()
        {
            checkOutActive = true;
            checkInActive = false;
            checkInButtonDisabled = false;
            checkOutButtonDisabled = true;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            var resultOfMissingCheckIns = await _checkInQuery.GetActiveBookingsWithMissingCheckInsAsync();
            var resultOfMissingCheckOuts = await _checkOutQuery.GetFinishedBookingsWithMissingCheckOutsAsync();

            if (resultOfMissingCheckIns.IsSucces())
            {
                _missingCheckIns = resultOfMissingCheckIns.GetSuccess().OriginalType;
            }
            if (resultOfMissingCheckOuts.IsSucces())
            {
                _missingCheckOuts = resultOfMissingCheckOuts.GetSuccess().OriginalType;
            }
            
        }
        
    }
}
