using Application.ApplicationDto.Query;
using Application.ApplicationDto.Query.Responses;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;

namespace Application.Services.Query
{
    public class ReadAllResourcesQuery : IReadAllResourcesQuery
    {
        private readonly IResourceApiService _apiService;

        public ReadAllResourcesQuery(IResourceApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IResult<IEnumerable<ReadAllResourceQueryResponseDto>>> ReadAllResourcesAsync(ResourceFilterDto filter)
        {
            IResult<IEnumerable<ReadAllResourceByApiResponse>> response = await _apiService.ReadAllResourcesAsync(filter);

            if (response.IsSucces() == false)
            {
                // Get exception
                Exception ex = response.GetError().Exception!;

                // Return to UI
                return Result<IEnumerable<ReadAllResourceQueryResponseDto>>.Error(null!, ex);
            }
            else
            {
                // Get success
                IEnumerable<ReadAllResourceByApiResponse> succes = response.GetSuccess().OriginalType;

                List<ReadAllResourceQueryResponseDto> listOfSuccess = new List<ReadAllResourceQueryResponseDto>();

                // Mapping success
                foreach (var ressource in succes)
                {
                    ReadAllResourceQueryResponseDto succesDto = Mapper.Map<ReadAllResourceQueryResponseDto>(succes);

                    listOfSuccess.Add(succesDto);
                }
                
                // Return to UI
                return Result<IEnumerable<ReadAllResourceQueryResponseDto>>.Success(listOfSuccess);
            }
        }
    }
}
