using Application.ApplicationDto;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
	public class ResourceControllerImplementation : IResourceController
	{

		private readonly ICreateResourceCommandHandler _createResourceHandler;
		private readonly IReadResourceWithCriteriaQueryHandler _readResourceWithCriteriaQueryHandler;
		private readonly IReadResourceByIdQueryHandler _readResourceByIdQueryHandler;
		private readonly IUpdateResourceByIdCommandHandler _updateResourceByIdQueryHandler;
		private readonly IHttpContextAccessor _contextAccessor;

		public ResourceControllerImplementation(ICreateResourceCommandHandler createResourceHandler, IReadResourceWithCriteriaQueryHandler readResourceWithCriteriaQueryHandler, IReadResourceByIdQueryHandler readResourceByIdQueryHandler, IHttpContextAccessor contextAccessor, IUpdateResourceByIdCommandHandler updateResourceByIdQueryHandler)
		{
			_createResourceHandler = createResourceHandler;
			_readResourceWithCriteriaQueryHandler = readResourceWithCriteriaQueryHandler;
			_readResourceByIdQueryHandler = readResourceByIdQueryHandler;
			_contextAccessor = contextAccessor;
			_updateResourceByIdQueryHandler = updateResourceByIdQueryHandler;
		}

		public async Task<ResourceResponseDto> CreateResourceAsync(CreateResourceCommandDto body)
		{
			var result = await _createResourceHandler.HandleAsync(body);

			if (result.IsSucces() is false)
			{
				// Shoud always be succesfuld if not let ExceptionHandler Middleware handle it
				throw result.GetError().Exception!;
			}

			_contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.Created.ToInt();
			return result.GetSuccess().OriginalType;
		}

		public Task DeleteResourceByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ResourceResponseDto> GetResourceByIdAsync(int id)
		{
			var result = await _readResourceByIdQueryHandler.HandleAsync(id);

			if (result.IsSucces() is false)
			{
				// Shoud always be succesfuld if not let ExceptionHandler Middleware handle it
				throw result.GetError().Exception!;
			}

			_contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.OK.ToInt();
			return result.GetSuccess().OriginalType;
		}

		public async Task<ICollection<ResourceResponseDto>> GetResourcesAsync(string name = null, IEnumerable<string> type = null, int? location = null, bool? isAvailable = null, decimal? minPrice = null, decimal? maxPrice = null)
		{
			// Takes the criterias from monolith into a microservice dto because of NSWAGs limitations of converting query parameters
			ReadResourceListQueryDto criteria = new ReadResourceListQueryDto();
			{
				criteria.Name = name;
				criteria.Type = type;
				criteria.Location = location;
				criteria.IsAvailable = isAvailable;
				criteria.MinPrice = minPrice;
				criteria.MaxPrice = maxPrice;
			}

			var result = await _readResourceWithCriteriaQueryHandler.HandleAsync(criteria);

			if (result.IsSucces() is false)
			{
				// Shoud always be succesfuld if not let ExceptionHandler Middleware handle it
				throw result.GetError().Exception!;
			}

			_contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.OK.ToInt();
			return result.GetSuccess().OriginalType;
		}

		public async Task<ResourceResponseDto> UpdateResourceByIdAsync(int id, UpdateResourceByIdCommandDto body)
		{
			var result = await _updateResourceByIdQueryHandler.HandleAsync(id, body);

			if (result.IsSucces() is false)
			{
				// Shoud always be succesfuld if not let ExceptionHandler Middleware handle it
				throw result.GetError().Exception!;
			}

			_contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.OK.ToInt();
			return result.GetSuccess().OriginalType;
		}
	}	
}
