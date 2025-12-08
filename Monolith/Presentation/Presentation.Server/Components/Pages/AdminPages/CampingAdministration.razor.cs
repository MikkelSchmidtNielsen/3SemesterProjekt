using Application.ApplicationDto.Query;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class CampingAdministration
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now); // Displays the current date at top of page.


        IEnumerable<ReadBookingMissingCheckInQueryResponseDto> _missingCheckIns = new List<ReadBookingMissingCheckInQueryResponseDto>();
        IEnumerable<ReadBookingMissingCheckOutQueryResponseDto> _missingCheckOuts = new List<ReadBookingMissingCheckOutQueryResponseDto>();

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
