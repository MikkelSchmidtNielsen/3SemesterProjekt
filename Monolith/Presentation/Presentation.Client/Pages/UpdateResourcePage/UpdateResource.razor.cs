using Presentation.Shared.Models;

namespace Presentation.Client.Pages.UpdateResourcePage
{
	public partial class UpdateResource
	{
		List<UpdateResourceModel> resources = new List<UpdateResourceModel>
	{
		new UpdateResourceModel
		{
			Id = 1,
			Name = "Resource A",
			Type = "Type 1",
			BasePrice = 100.0m,
			Location = 1,
			Description = "This is resource A",
			IsAvailable = true,
			RowVersion = new byte[] { 1, 0, 0, 0 }
		},
		new UpdateResourceModel
		{
			Id = 2,
			Name = "Resource B",
			Type = "Type 2",
			BasePrice = 200.0m,
			Location = 2,
			Description = "This is resource B",
			IsAvailable = false,
			RowVersion = new byte[] { 1, 0, 0, 1 }
		},
		new UpdateResourceModel
		{
			Id = 3,
			Name = "Resource C",
			Type = "Type 1",
			BasePrice = 150.0m,
			Location = 1,
			Description = "This is resource C",
			IsAvailable = true,
			RowVersion = new byte[] { 1, 0, 0, 2 }
		},
		new UpdateResourceModel
		{
			Id = 4,
			Name = "Resource D",
			Type = "Type 3",
			BasePrice = 300.0m,
			Location = 3,
			Description = "This is resource D",
			IsAvailable = true,
			RowVersion = new byte[] { 1, 0, 0, 3 }
		}
	};
		// TODO Hent data via API

		UpdateResourceModel? selectedResource;


		private async Task OnModelSubmitAsync(UpdateResourceModel registerModel)
		{ 
			// TODO
		
		}
	}
}
