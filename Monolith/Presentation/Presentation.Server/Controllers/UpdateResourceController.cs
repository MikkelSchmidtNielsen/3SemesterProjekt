using Common;
using Common.ResultInterfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Shared.Models;
using System.Collections.Generic;

namespace Presentation.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UpdateResourceController : ControllerBase
	{
		[HttpGet]
		public async Task<IEnumerable<UpdateResourceModel>> GetAllResources()
		{
			return new List<UpdateResourceModel>
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
		}

		[HttpPut("{id}")]
		public Task<UpdateResourceModel> UpdateResource(int id, [FromBody]UpdateResourceModel resource)
		{
			throw new NotImplementedException();
		}
	}
}
