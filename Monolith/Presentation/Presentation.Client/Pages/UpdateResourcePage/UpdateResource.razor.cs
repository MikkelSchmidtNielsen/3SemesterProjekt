using Common.ResultInterfaces;
using Presentation.Client.Services.Interfaces;
using Presentation.Shared.Models;
using System.Resources;

namespace Presentation.Client.Pages.UpdateResourcePage
{
	public partial class UpdateResource
	{
		IEnumerable<UpdateResourceModel>? _resources;

		protected override async Task OnInitializedAsync()
		{
			IResult<IEnumerable<UpdateResourceModel>> apiResponse = await _updateService.GetAllResourcesAsync();

			if (apiResponse.IsSucces() is false)
			{
				_resources = Array.Empty<UpdateResourceModel>();
			}
			else
			{
				_resources = apiResponse.GetSuccess().OriginalType;
			}
		}
	
		UpdateResourceModel? _selectedResource;


		private async Task OnModelSubmitAsync(UpdateResourceModel updateModel)
		{ 
			IResult<UpdateResourceModel> apiResponse = await _updateService.UpdateResourceAsync(updateModel);
			
			if (apiResponse.IsSucces())
			{
				await SuccessDialog();
				_selectedResource.RowVersion = apiResponse.GetSuccess().OriginalType.RowVersion;
			}
			else
			{
				await ErrorDialog(apiResponse.GetError().Exception!);
			}
		}
	}
}
