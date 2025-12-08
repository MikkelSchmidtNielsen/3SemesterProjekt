using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Query;
using Common;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Infrastructure.InternalApiCalls.UserAuthenticationApi;
using Refit;

namespace Infrastructure.InternalApiCalls.ResourceApi
{
    public class ResourceApiService : IResourceApiService
    {
        private readonly IResourceApi _resourceApi;

        public ResourceApiService(IResourceApi resourceApi)
        {
            _resourceApi = resourceApi;
        }

        public async Task<IResult<CreateResourceByApiResponseDto>> CreateResourceAsync(CreateResourceCommandDto dto)
        {
            try
            {
                CreateResourceByApiResponseDto result = await _resourceApi.CreateResourceAsync(dto);

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
                        Message = "Uventet fejl fra API",
                    };
                }

                // A custom Exception, returns BadReponse error message, status code and original ApiException message all in one exception
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

        public async Task<IResult<ReadResourceByIdByApiResponseDto>> ReadResourceByIdAsync(int id)
        {
            try
            {
                ReadResourceByIdByApiResponseDto result = await _resourceApi.ReadResourceByIdAsync(id);

                return Result<ReadResourceByIdByApiResponseDto>.Success(result);
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
                        Message = "Uventet fejl fra API",
                    };
                }

                // A custom Exception, returns BadReponse error message, status code and original ApiException message all in one exception
                ApiErrorException apiErrorException = new ApiErrorException(
                    apiErrorMessage: error?.Message,
                    statusCode: (int)ex.StatusCode,
                    original: ex
                );

                return Result<ReadResourceByIdByApiResponseDto>.Error(
                    originalType: null,
                    exception: apiErrorException
                );
            }
            catch (Exception ex)
            {
                // If something breaks which is not from an Api Response return
                return Result<ReadResourceByIdByApiResponseDto>.Error(originalType: null, exception: ex);
            }
        }

        public async Task<IResult<IEnumerable<ReadResourceByApiQueryResponseDto>>> ReadAllResourcesAsync(ResourceFilterDto uiFilter)
        {
            InternalResourceApiFilterDto filter = Mapper.Map<InternalResourceApiFilterDto>(uiFilter);

            try
            {
                var result = await _resourceApi.ReadAllResourcesAsync(filter);

                return Result<IEnumerable<ReadResourceByApiQueryResponseDto>>.Success(result);
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
                        Message = "Uventet fejl fra API",
                    };
                }

                // A custom Exception, returns BadReponse error message, status code and original ApiException message all in one exception
                ApiErrorException apiErrorException = new ApiErrorException(
                    apiErrorMessage: error?.Message,
                    statusCode: (int)ex.StatusCode,
                    original: ex
                );

                return Result<IEnumerable<ReadResourceByApiQueryResponseDto>>.Error(
                    originalType: null,
                    exception: apiErrorException
                );
            }
            catch (Exception ex)
            {
                // If something breaks which is not from an Api Response return
                return Result<IEnumerable<ReadResourceByApiQueryResponseDto>>.Error(originalType: null, exception: ex);
            }
        }

		public async Task<IResult<UpdateResourceByApiResponseDto>> UpdateAsync(UpdateResourceCommandDto dto)
		{
			try
			{
                UpdateResourceByApiResponseDto result = await _resourceApi.UpdateResourceAsync(dto.Id, dto);

				return Result<UpdateResourceByApiResponseDto>.Success(result);
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
						Message = "Uventet fejl fra API",
					};
				}

				// A custom Exception, returns BadReponse error message, status code and original ApiException message all in one exception
				ApiErrorException apiErrorException = new ApiErrorException(
					apiErrorMessage: error?.Message,
					statusCode: (int)ex.StatusCode,
					original: ex
				);

				return Result<UpdateResourceByApiResponseDto>.Error(
					originalType: null,
					exception: apiErrorException
				);
			}
			catch (Exception ex)
			{
				// If something breaks which is not from an Api Response return
				return Result<UpdateResourceByApiResponseDto>.Error(originalType: null, exception: ex);
			}
		}
	}
}
