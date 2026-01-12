using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Presentation.Server.Components.Pages.LoginPages
{
	public partial class RegisterGuestComponent
	{
		[Parameter, EditorRequired]
		public RegisterModel RegisterModelComponent { get; set; }

		[Parameter, EditorRequired]
		public EventCallback<RegisterModel> OnModelSubmit { get; set; }

		private async Task OnModelSubmitAsync(RegisterModel model)
		{
			await OnModelSubmit.InvokeAsync(RegisterModelComponent);
		}

        private void HandleShortCut(KeyboardEventArgs e)
        {
            if ((e.Key == "z" || e.Key == "Z") && e.CtrlKey)
            {
                RegisterModelComponent.FirstName = "Jan";
				RegisterModelComponent.LastName = "Pan Nees";
				RegisterModelComponent.PhoneNumber = "12345678";
				RegisterModelComponent.Address = "Japanvej 7";
				RegisterModelComponent.Language = "Dansk";
				RegisterModelComponent.Country = "Danmark";
            }
        }
    }
}
