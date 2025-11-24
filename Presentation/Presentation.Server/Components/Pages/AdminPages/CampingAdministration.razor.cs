using Application.ApplicationDto.Query;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class CampingAdministration
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        bool checkInActive = true;
        bool checkOutActive = false;

        IEnumerable<BookingMissingCheckInResponseDto> _missingCheckIns = new List<BookingMissingCheckInResponseDto>();

        protected override async Task OnInitializedAsync()
        {
            var resultOfMissingCheckIns = await _checkInQuery.GetActiveBookingsWithMissingCheckInsAsync();

            if (resultOfMissingCheckIns.IsSucces())
            {
                _missingCheckIns = resultOfMissingCheckIns.GetSuccess().OriginalType;
            }
        }
    }
}
