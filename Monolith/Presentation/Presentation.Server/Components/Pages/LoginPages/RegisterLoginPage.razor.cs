
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
                _submitButtonVisible = false;
                SendNotification(NotificationSeverity.Info, "Velkommen tilbage", "Der er blevet afsendt en engangskode til den indtastede email.");
				// Shows the one-time password input UI
				_accountAlreadyExists = true;
				return;
			}

			SendNotification(NotificationSeverity.Info, "Velkommen", "Du kan nu oprette en konto med den indtastede email.");

			// Show the rest of the input UI
			_accountDoesNotAlreadyExist = true;

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
		private bool _accountDoesNotAlreadyExist = false;
        private bool _accountAlreadyExists = false;
		private bool _submitButtonVisible = true;
		private string? _oneTimePassword;
    }
}
