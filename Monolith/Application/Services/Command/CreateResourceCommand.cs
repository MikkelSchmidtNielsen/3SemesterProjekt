using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;

namespace Application.Services.Command
{
    public class CreateResourceCommand : ICreateResourceCommand
    {
        private readonly IResourceApiService _apiService;

        public CreateResourceCommand(IResourceApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IResult<CreateResourceUIResponseDto>> CreateResourceAsync(UICreateResourceDto dto)
        {
            CreateResourceCommandDto commandDto = Mapper.Map<CreateResourceCommandDto>(dto);

            IResult<CreateResourceByApiResponseDto> response = await _apiService.CreateResourceAsync(commandDto);

            if (response.IsSucces() == false)
            {
                // Get exception
                Exception ex = response.GetError().Exception!;

                // Return to UI
                return Result<CreateResourceUIResponseDto>.Error(null!, ex);
            }
            else
            {
                // Get success
                CreateResourceByApiResponseDto succes = response.GetSuccess().OriginalType;

                // Mapping success
                CreateResourceUIResponseDto succesDto = Mapper.Map<CreateResourceUIResponseDto>(succes);

                // Returb to UI
                return Result<CreateResourceUIResponseDto>.Success(succesDto);
            }
        }
    }
}
