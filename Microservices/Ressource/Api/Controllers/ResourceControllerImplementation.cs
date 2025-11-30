using Application.ApplicationDto;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;

namespace Api.Controllers
{
	public class ResourceControllerImplementation : IResourceController
	{

		private readonly ICreateResourceHandler _createResourceHandler;
		private readonly IReadResourceWithCriteriaQueryHandler _readResourceWithCriteriaQueryHandler;
		private readonly IReadResourceByIdQueryHandler _readResourceByIdQueryHandler;

		public ResourceControllerImplementation(ICreateResourceHandler createResourceHandler, IReadResourceWithCriteriaQueryHandler readResourceWithCriteriaQueryHandler)
		{
			_createResourceHandler = createResourceHandler;
			_readResourceWithCriteriaQueryHandler = readResourceWithCriteriaQueryHandler;
		}

		public async Task<ICollection<ResourceResponseDto>> ResourcesAllAsync(string name = null, IEnumerable<string> type = null, int? location = null, bool? isAvailable = null, decimal? minPrice = null, decimal? maxPrice = null)
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

			ICollection<ResourceResponseDto> apiReponse;

			// Checks if the originaltype has been set. Only in case of Error will it not be set
			if (applicationResponse.OriginalType is null)
			{
				// Sets a default Reponse with all values as null
				apiReponse = new List<ResourceResponseDto>();
			}
			else
			{
				// Converts IEnumerable to ICollection
				apiReponse = applicationResponse.OriginalType.ToList();
			}

			return apiReponse;
		}

		public Task ResourcesDELETEAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ResourceResponseDto> ResourcesGETAsync(int id)
		{
			var applicationResponse = await _readResourceByIdQueryHandler.HandleAsync(id);

			ResourceResponseDto apiReponse;

			// Checks if the originaltype has been set. Only in case of Error will it not be set
			if (applicationResponse.OriginalType is null)
			{
				// Sets a default Reponse with all values as null
				apiReponse = new ResourceResponseDto();
			}
			else
			{
				apiReponse = applicationResponse.OriginalType;
			}

			apiReponse.StatusCode = applicationResponse.StatusCode;

			return apiReponse;
		}

		public async Task<ResourceResponseDto> ResourcesPOSTAsync(CreateResourceCommandDto body)
		{
			var applicationResponse = await _createResourceHandler.HandleAsync(body);

			ResourceResponseDto apiReponse;

			// Checks if the originaltype has been set. Only in case of Error will it not be set
			if (applicationResponse.OriginalType is null)
			{
				// Sets a default Reponse with all values as null
				apiReponse = new ResourceResponseDto();
			}
			else
			{
				apiReponse = applicationResponse.OriginalType;
			}

			apiReponse.StatusCode = applicationResponse.StatusCode;

			return apiReponse;
		}

		public Task<ResourceResponseDto> ResourcesPUTAsync(int id, UpdateResourceByIdCommandDto body)
		{
			throw new NotImplementedException();
		}
	}
}
