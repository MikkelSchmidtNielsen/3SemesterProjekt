
using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Azure;
using Common;
using Common.ResultInterfaces;
using Radzen;

namespace Presentation.Server.Components.Pages.LoginPages
{
	public partial class RegisterLoginPage
	{
		private async Task OnEmailSumbitAsync()
		{
			if (string.IsNullOrEmpty(_email)) return;
			ReadGuestCheckIfEmailIsAvailableQueryDto dto = new ReadGuestCheckIfEmailIsAvailableQueryDto() { Email = _email };

			// Submit button is disabled, so multiple request dont overlap
			isEmailDisabled = true;

			var response = await _query.HandleAsync(dto);

			if (response.IsSucces() is false)
			{
				// Reverted back to false, so use can reinput thier email
				isEmailDisabled = false;

				SendNotification(NotificationSeverity.Error, "Error", "Email er allerde registeret til en anden konto");
				return;
			}

			SendNotification(NotificationSeverity.Success, "Success", "Email er ikke registeret endnu");

			// Show the rest of the input UI
			_emailIsValidated = true;

		}

		private void SendNotification(NotificationSeverity severity, string summary, string detail)
		{
			NotificationMessage message = new NotificationMessage {Severity = severity, Summary = summary, Detail = detail, Duration = 4000};

			NotificationService.Notify(message);
		}

		private async Task OnModelSubmitAsync(RegisterModel registerModel)
		{
			string number = new string(registerModel.PhoneNumber!.Where(char.IsDigit).ToArray());

			int numberAsInt = Convert.ToInt32(number);

            var dto = Mapper.Map<GuestCreateRequestDto>(registerModel);
            dto.Email = _email;
			dto.PhoneNumber = numberAsInt;

            var result = await _command.CreateGuestAsync(dto);

            if (result.IsSucces() is false)
            {
                SendNotification(NotificationSeverity.Error, "Fejl", "Konto med den email eksitere allerede");
                return;
            }

            SendNotification(NotificationSeverity.Success, "Succes", "Konto er nu oprettet");
        }

		private string? _email;
		private bool isEmailDisabled = false;
		private RegisterModel _registerModel = new();
		private bool _emailIsValidated = false;
	}
}
