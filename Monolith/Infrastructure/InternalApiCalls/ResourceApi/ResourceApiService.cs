using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Infrastructure.InternalApiCalls.UserAuthenticationApi;
using Refit;

namespace Infrastructure.InternalApiCalls.ResourceApi
{
    public class ResourceApiService : IResourceApiService
    {
        private readonly IResourceApi _api;

        public ResourceApiService(IResourceApi api)
        {
            _api = api;
        }

        public async Task<IResult<CreateResourceByApiResponseDto>> CreateResourceAsync(CreateResourceCommandDto dto)
        {
            try
            {
                CreateResourceByApiResponseDto result = await _api.RegisterUserAsync(dto);

                return Result<CreateResourceByApiResponseDto>.Success(result);
            }
            catch (ApiException ex)
            {
                BadResponseDto? error = null;

                // If its anything else but a 201 response (409 or 500 reponse code) try gets its value from api call
                try
                {
                    // Try to parse Content from Json to BadReponse
                    error = await ex.GetContentAsAsync<BadResponseDto>();
                }
                catch (Exception)
                {
                    // If parsing from Json didnt work manual create BadResponse
                    error = new BadResponseDto
                    {
                        Message = "Unexpected error format from API",
                    };
                }

                // A custom Exception, so I can get BadReponse error message, status code and original ApiException message all in one exception
                ApiErrorException apiErrorException = new ApiErrorException(
                    apiErrorMessage: error?.Message,
                    statusCode: (int)ex.StatusCode,
                    original: ex
                );

                return Result<CreateResourceByApiResponseDto>.Error(
                    originalType: null,
                    exception: apiErrorException
                );
            }
            catch (Exception ex)
            {
                // If something breaks which is not from an Api Response return
                return Result<CreateResourceByApiResponseDto>.Error(originalType: null, exception: ex);
            }
        }
    }
}
