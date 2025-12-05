using Presentation.Client.Services.Interfaces;
using Presentation.Shared.Models;
using System.Resources;

namespace Presentation.Client.Pages.UpdateResourcePage
{
	public partial class UpdateResource
	{
		IEnumerable<UpdateResourceModel>? resources;

		protected override async Task OnInitializedAsync()
		{
			resources = (IEnumerable<UpdateResourceModel>)await _updateService.GetAllResourcesAsync();
		}
	
		UpdateResourceModel? selectedResource;


		private async Task OnModelSubmitAsync(UpdateResourceModel registerModel)
		{ 
			// TODO
		
		}
	}
}
