using Application.ApplicationDto.Query;
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

        public async Task<IResult<IEnumerable<ReadResourceQueryResponseDto>>> ReadAllResourcesAsync(ResourceFilterDto filter)
        {
            IResult<IEnumerable<ReadResourceByApiQueryResponseDto>> response = await _apiService.ReadAllResourcesAsync(filter);

            if (response.IsSucces() == false)
            {
                // Get exception
                Exception ex = response.GetError().Exception!;

                // Return to UI
                return Result<IEnumerable<ReadResourceQueryResponseDto>>.Error(null!, ex);
            }
            else
            {
                // Get success
                IEnumerable<ReadResourceByApiQueryResponseDto> succes = response.GetSuccess().OriginalType;

                List<ReadResourceQueryResponseDto> listOfSuccess = new List<ReadResourceQueryResponseDto>();

                // Mapping success
                foreach (var resource in succes)
                {
                    ReadResourceQueryResponseDto succesDto = Mapper.Map<ReadResourceQueryResponseDto>(resource);

                    listOfSuccess.Add(succesDto);
                }
                
                // Return to UI
                return Result<IEnumerable<ReadResourceQueryResponseDto>>.Success(listOfSuccess);
            }
        }
    }
}
