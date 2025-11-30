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
		private readonly IHttpContextAccessor _contextAccessor;

		public ResourceControllerImplementation(ICreateResourceCommandHandler createResourceHandler, IReadResourceWithCriteriaQueryHandler readResourceWithCriteriaQueryHandler, IReadResourceByIdQueryHandler readResourceByIdQueryHandler, IHttpContextAccessor contextAccessor)
		{
			_createResourceHandler = createResourceHandler;
			_readResourceWithCriteriaQueryHandler = readResourceWithCriteriaQueryHandler;
			_readResourceByIdQueryHandler = readResourceByIdQueryHandler;
			_contextAccessor = contextAccessor;
		}

		public async Task<ResourceResponseDto> CreateResourceAsync(CreateResourceCommandDto body)
		{
			var result = await _createResourceHandler.HandleAsync(body);

			if (result.IsSucces() is false)
			{
				// Shoud always be succesfuld if not let ExceptionHandler Middleware handle it
				throw new Exception();
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
				throw new Exception();
			}

			_contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.OK.ToInt();
			return result.GetSuccess().OriginalType;
		}

		public async Task<ICollection<ResourceResponseDto>> GetResourcesAsync(string name = null, IEnumerable<string> type = null, int? location = null, bool? isAvailable = null, decimal? minPrice = null, decimal? maxPrice = null)
		{
			ReadResourceListQueryDto criterias = new ReadResourceListQueryDto();
			{
				criterias.Name = name;
				criterias.Type = type;
				criterias.Location = location;
				criterias.IsAvailable = isAvailable;
				criterias.MinPrice = minPrice;
				criterias.MaxPrice = maxPrice;
			}

			var result = await _readResourceWithCriteriaQueryHandler.HandleAsync(criterias);

			if (result.IsSucces() is false)
			{
				// Shoud always be succesfuld if not let ExceptionHandler Middleware handle it
				throw new Exception();
			}

			_contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.OK.ToInt();
			return result.GetSuccess().OriginalType;
		}

		public Task<ResourceResponseDto> UpdateResourceByIdAsync(int id, UpdateResourceByIdCommandDto body)
		{
			throw new NotImplementedException();
		}
	}	
}
