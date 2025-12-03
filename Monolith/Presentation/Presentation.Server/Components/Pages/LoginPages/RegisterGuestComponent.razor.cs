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
	}
}
