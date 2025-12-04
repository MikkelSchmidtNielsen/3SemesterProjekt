using Common.ResultInterfaces;
using Presentation.Client.Services.Interfaces;
using Presentation.Shared.Models;
using Presentation.Shared.Refit;
using System.Runtime.CompilerServices;

namespace Presentation.Client.Services.Implementation
{
	public class UpdateResourceService : IUpdateResourceService
	{
		private readonly IUpdateResourceApi _api;

		public UpdateResourceService(IUpdateResourceApi api)
		{
			_api = api;
		}

		public Task<IResult<UpdateResourceModel>> GetAllResources()
		{
			_api.GetAllResources();
			throw new NotImplementedException();
		}

		public Task<IResult<UpdateResourceModel>> UpdateResource(UpdateResourceModel resource)
		{
			throw new NotImplementedException();
		}
	}
}
