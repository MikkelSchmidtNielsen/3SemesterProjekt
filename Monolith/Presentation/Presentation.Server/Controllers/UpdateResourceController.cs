using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Shared.Models;
using System.Collections.Generic;
using System.Net;

namespace Presentation.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UpdateResourceController : ControllerBase
	{
		private readonly IReadAllResourcesQuery _query;
		private readonly IHttpContextAccessor _httpContext;
		private readonly IUpdateResourceCommand _command;

		public UpdateResourceController(IReadAllResourcesQuery query, IHttpContextAccessor httpContext, IUpdateResourceCommand command)
		{
			_query = query;
			_httpContext = httpContext;
			_command = command;
		}

		[HttpGet]
		public async Task<IEnumerable<UpdateResourceModel>> GetAllResources()
		{
			IResult<IEnumerable<ReadResourceQueryResponseDto>> result = await _query.ReadAllResourcesAsync(new ResourceFilterDto());

			if (result.IsError())
			{
				_httpContext.HttpContext!.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				if(result.GetError() is ApiErrorException apiErrorException)
				{
					_httpContext.HttpContext.Response.StatusCode = apiErrorException.StatusCode;
				}

				await _httpContext.HttpContext.Response.WriteAsJsonAsync(result.GetError().Exception!.Message);
			}

			List<UpdateResourceModel> resultList = new List<UpdateResourceModel>();

			foreach (var resource in result.GetSuccess().OriginalType)
			{
				resultList.Add(Mapper.Map<UpdateResourceModel>(resource));
			}

			_httpContext.HttpContext!.Response.StatusCode = (int)HttpStatusCode.OK;
			return resultList;
		}

		[HttpPut("{id}")]
		public async Task<UpdateResourceModel> UpdateResource(int id, [FromBody]UpdateResourceModel resource)
		{
			UpdateResourceCommandDto dto = Mapper.Map<UpdateResourceCommandDto>(resource);

			IResult<UpdateResourceResponseDto> result = await _command.HandleAsync(dto);

			if (result.IsError())
			{
				_httpContext.HttpContext!.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				if (result.GetError() is ApiErrorException apiErrorException)
				{
					_httpContext.HttpContext.Response.StatusCode = apiErrorException.StatusCode;
				}
				return new UpdateResourceModel();
			}

			_httpContext.HttpContext!.Response.StatusCode = (int)HttpStatusCode.OK;

			return Mapper.Map<UpdateResourceModel>(result.GetSuccess().OriginalType);
		}
	}
}
