using Application.ApplicationDto.Query;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class CampingAdministration
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        bool checkInActive = true;
        bool checkOutActive = false;

        IEnumerable<BookingMissingCheckInQueryDto> _missingCheckIns = new List<BookingMissingCheckInQueryDto>();

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
