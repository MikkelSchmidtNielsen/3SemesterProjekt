using Common.ResultInterfaces;
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
			IResult<IEnumerable<UpdateResourceModel>> apiResponse = await _updateService.GetAllResourcesAsync();

			if (apiResponse.IsSucces() is false)
			{
				resources = Array.Empty<UpdateResourceModel>();
			}
			else
			{
				resources = apiResponse.GetSuccess().OriginalType;
			}
		}
	
		UpdateResourceModel? selectedResource;


		private async Task OnModelSubmitAsync(UpdateResourceModel updateModel)
		{ 
			IResult<UpdateResourceModel> apiReponse = await _updateService.UpdateResourceAsync(updateModel);
			
			if (apiReponse.IsSucces())
			{
				await SuccessDialog();
			}
			else
			{
				await ErrorDialog(apiReponse.GetError().Exception!);
			}
		}
	}
}
