using Application.ApplicationDto.Query;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class CampingAdministration
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        bool checkInActive = true;
        bool checkOutActive = false;

        IEnumerable<BookingMissingCheckInResponseDto> _missingCheckIns = new List<BookingMissingCheckInResponseDto>();
        IEnumerable<BookingMissingCheckOutResponseDto> _missingCheckOuts = new List<BookingMissingCheckOutResponseDto>();

        protected override async Task OnInitializedAsync()
        {
            var resultOfMissingCheckIns = await _checkInQuery.GetActiveBookingsWithMissingCheckInsAsync();
            var resultOfMissingCheckOuts = await _checkOutQuery

            if (resultOfMissingCheckIns.IsSucces())
            {
                _missingCheckIns = resultOfMissingCheckIns.GetSuccess().OriginalType;
            }
            
        }
    }
}
