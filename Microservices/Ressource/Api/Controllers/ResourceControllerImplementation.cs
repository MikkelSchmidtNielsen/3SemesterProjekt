using Application.ApplicationDto;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ExtensionMethods;

namespace Api.Controllers
{
	public class ResourceControllerImplementation : IResourceController
	{

		private readonly ICreateResourceHandler _createResourceHandler;
		private readonly IReadResourceWithCriteriaQueryHandler _readResourceWithCriteriaQueryHandler;
		private readonly IReadResourceByIdQueryHandler _readResourceByIdQueryHandler;

		public ResourceControllerImplementation(ICreateResourceHandler createResourceHandler, IReadResourceWithCriteriaQueryHandler readResourceWithCriteriaQueryHandler, IReadResourceByIdQueryHandler readResourceByIdQueryHandler)
		{
			_createResourceHandler = createResourceHandler;
			_readResourceWithCriteriaQueryHandler = readResourceWithCriteriaQueryHandler;
			_readResourceByIdQueryHandler = readResourceByIdQueryHandler;
		}

		public async Task<ApiResponseDto> CreateResourceAsync(CreateResourceCommandDto body)
		{
			var applicationResponse = await _createResourceHandler.HandleAsync(body);

			ResourceResponseDto resourceReponse;
			string message;

			// Checks if the originaltype has been set. Only in case of Error will it not be set
			if (applicationResponse.OriginalType is null)
			{
				// Sets a default Reponse with all values as null
				resourceReponse = new ResourceResponseDto();
				message = applicationResponse.GetError().Exception!.Message;
			}
			else
			{
				resourceReponse = applicationResponse.OriginalType;
				message = "Succes";
			}

			// Creates a ApiReponse
			ApiResponseDto apiResponseDto = new ApiResponseDto();
			{
				apiResponseDto.StatusCode = applicationResponse.StatusCode.ToInt();
				apiResponseDto.Message = message;
				apiResponseDto.Responses = resourceReponse.ToCollection(); // Extension Method defined in common
			}

			return apiResponseDto;
		}

		public Task<ApiResponseDto> DeleteResourceByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResponseDto> GetResourceByIdAsync(int id)
		{
			var applicationResponse = await _readResourceByIdQueryHandler.HandleAsync(id);

			ResourceResponseDto resourceReponse;
			string message;

			// Checks if the originaltype has been set. Only in case of Error will it not be set
			if (applicationResponse.OriginalType is null)
			{
				// Sets a default Reponse with all values as null
				resourceReponse = new ResourceResponseDto();
				message = applicationResponse.GetError().Exception!.Message;
			}
			else
			{
				resourceReponse = applicationResponse.OriginalType;
				message = "Succes";
			}

			// Creates a ApiReponse
			ApiResponseDto apiResponseDto = new ApiResponseDto();
			{
				apiResponseDto.StatusCode = applicationResponse.StatusCode.ToInt();
				apiResponseDto.Message = message;
				apiResponseDto.Responses = resourceReponse.ToCollection(); // Extension Method defined in common
			}

			return apiResponseDto;
		}

		public async Task<ApiResponseDto> GetResourcesAsync(string name = null, IEnumerable<string> type = null, int? location = null, bool? isAvailable = null, decimal? minPrice = null, decimal? maxPrice = null)
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

			var applicationResponse = await _readResourceWithCriteriaQueryHandler.HandleAsync(criterias);

			ICollection<ResourceResponseDto> resourceReponse;
			string message;

			// Checks if the originaltype has been set. Only in case of Error will it not be set
			if (applicationResponse.OriginalType is null)
			{
				// Sets a default Reponse with all values as null
				resourceReponse = new List<ResourceResponseDto>();
				message = applicationResponse.GetError().Exception!.Message;
			}
			else
			{
				// Converts IEnumerable to ICollection
				resourceReponse = applicationResponse.OriginalType;
				message = "Succes";
			}

			// Creates a ApiReponse
			ApiResponseDto apiResponseDto = new ApiResponseDto();
			{
				apiResponseDto.StatusCode = applicationResponse.StatusCode.ToInt();
				apiResponseDto.Message = message;
				apiResponseDto.Responses = resourceReponse;
			}

			return apiResponseDto;
		}


		public Task<ApiResponseDto> UpdateResourceByIdAsync(int id, UpdateResourceByIdCommandDto body)
		{
			throw new NotImplementedException();
		}
	}
}
