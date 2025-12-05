using Common.ResultInterfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Shared.Models;

namespace Presentation.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UpdateResourceController : ControllerBase
	{
		[HttpGet]
		public Task<IResult<IEnumerable<UpdateResourceModel>>> GetAllResources()
		{
			throw new NotImplementedException();
		}

		[HttpPut("{id}")]
		public Task<IResult<UpdateResourceModel>> UpdateResource(int id, [FromBody]UpdateResourceModel resource)
		{
			throw new NotImplementedException();
		}
	}
}
