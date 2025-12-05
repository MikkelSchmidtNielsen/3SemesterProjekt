using Common.ResultInterfaces;
using Presentation.Shared.Models;

namespace Presentation.Client.Services.Interfaces
{
	public interface IUpdateResourceService
	{
		Task<IResult<IEnumerable<UpdateResourceModel>>> GetAllResourcesAsync();
		Task<IResult<UpdateResourceModel>> UpdateResourceAsync(UpdateResourceModel resource);
	}
}
