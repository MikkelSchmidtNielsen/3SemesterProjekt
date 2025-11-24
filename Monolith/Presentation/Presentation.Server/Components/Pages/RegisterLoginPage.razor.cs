
namespace Presentation.Server.Components.Pages
{
	public partial class RegisterLoginPage
	{
		protected override Task OnInitializedAsync()
		{
			return base.OnInitializedAsync();
		}


		private class RegisterModel()
		{
			public string FirstName { get; set; }
			public string? LastName { get; set; }
			public int? PhoneNumber { get; set; }
			public string? Email { get; set; }
			public string? Country { get; set; }
			public string? Language { get; set; }
			public string? Address { get; set; }
		}
	}
}
