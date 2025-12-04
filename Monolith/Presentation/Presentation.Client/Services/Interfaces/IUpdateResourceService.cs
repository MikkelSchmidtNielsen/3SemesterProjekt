using Common.ResultInterfaces;
using Presentation.Shared.Models;

namespace Presentation.Client.Services.Interfaces
{
	public interface IUpdateResourceService
	{
		Task<IResult<UpdateResourceModel>> GetAllResources();
		Task<IResult<UpdateResourceModel>> UpdateResource(UpdateResourceModel resource);
	}
}
