using Microsoft.AspNetCore.Components;
using Presentation.Shared.Models;
using Radzen;
using System.Globalization;

namespace Presentation.Client.Pages.UpdateResourcePage
{
	public partial class UpdateResourceComponent
	{
		[Parameter, EditorRequired]
		public UpdateResourceModel resource { get; set; }

		[Parameter, EditorRequired]
		public EventCallback<UpdateResourceModel> OnModelSubmit { get; set; }

		private async Task OnModelSubmitAsync(UpdateResourceModel model, DialogService ds)
		{
			ds.Close();
			await OnModelSubmit.InvokeAsync(model);
		}

		TextInfo _textInfo = new CultureInfo("da-DK", false).TextInfo;
		List<string> _resourceTypes = new List<string>() { "Hytte", "Campingvognsplads", "Teltplads" };
	}
}
