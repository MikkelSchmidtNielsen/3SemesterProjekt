using Application.ApplicationDto.Command.Responses;
using Application.ApplicationDto.Query.Responses;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;

namespace Application.Services.Query
{
    public class ReadResourceByIdQuery : IReadResourceByIdQuery
    {
        private readonly IResourceApiService _apiService;

        public ReadResourceByIdQuery(IResourceApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IResult<ReadResourceByIdQueryResponseDto>> ReadResourceByIdAsync(int id)
        {
            IResult<ReadResourceByIdByApiResponseDto> response = await _apiService.ReadResourceByIdAsync(id);

            if (response.IsSucces() == false)
            {
                // Get exception
                Exception ex = response.GetError().Exception!;

                // Return to UI
                return Result<ReadResourceByIdQueryResponseDto>.Error(null!, ex);
            }
            else
            {
                // Get success
                ReadResourceByIdByApiResponseDto succes = response.GetSuccess().OriginalType;

                // Mapping success
                ReadResourceByIdQueryResponseDto succesDto = Mapper.Map<ReadResourceByIdQueryResponseDto>(succes);

                // Return to UI
                return Result<ReadResourceByIdQueryResponseDto>.Success(succesDto);
            }
        }
    }
}
