using Common.ResultInterfaces;
using Presentation.Shared.Models;
using Refit;

namespace Presentation.Shared.Refit
{
	public interface IUpdateResourceApi
	{
		[Get("/api/UpdateResource")]
		Task<IEnumerable<UpdateResourceModel>> GetAllResources();

		[Put("/api/UpdateResource/{id}")]
		Task<UpdateResourceModel> UpdateResource(int id, [Body] UpdateResourceModel resource);
	}
}
