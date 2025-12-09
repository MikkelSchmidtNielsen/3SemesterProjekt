using Application.ApplicationDto.Query;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Refit;

namespace Infrastructure.InternalApiCalls.UserAuthenticationApi
{
	public class UserAuthenticationApiService : IUserAuthenticationApiService
	{
		private readonly IUserAuthenticationApi _userAuthenticationApi;

		public UserAuthenticationApiService(IUserAuthenticationApi userAuthenticationApi)
		{
			_userAuthenticationApi = userAuthenticationApi;
		}

        public async Task<IResult<CreateUserByApiReponseDto>> RegisterUserAsync(string email)
		{
			try
			{
				// Successful 201 -> return JWT
				string jwt = await _userAuthenticationApi.RegisterUserAsync(email);

				CreateUserByApiReponseDto response = new() { JwtToken = jwt };
				return Result<CreateUserByApiReponseDto>.Success(response);
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

				// A custom Exception, so I can get BadReponse error message, status code and original ApiException message all in one exception
				ApiErrorException apiErrorException = new ApiErrorException(
					apiErrorMessage: error?.Message,
					statusCode: (int)ex.StatusCode,
					original: ex
				);

				return Result<CreateUserByApiReponseDto>.Error(
					originalType: null,
					exception: apiErrorException
				);
			}
			catch (Exception ex)
			{
				// If something breaks which is not from an Api Response return
				return Result<CreateUserByApiReponseDto>.Error(originalType: null, exception: ex);
			}
		}
        public async Task RequestOtpAsync(string email)
        {
			try
			{
				await _userAuthenticationApi.RequestOtpAsync(email);
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

                // A custom Exception, so I can get BadReponse error message, status code and original ApiException message all in one exception
                ApiErrorException apiErrorException = new ApiErrorException(
                    apiErrorMessage: error?.Message,
                    statusCode: (int)ex.StatusCode,
                    original: ex
                );
            }
        }

        public async Task<IResult<ValidateUserByApiResponseDto>> ValidateUserAsync(ValidateUserQueryDto dto)
        {
            try
            {
                ValidateUserByApiResponseDto response = await _userAuthenticationApi.ValidateUserAsync(dto);
                return Result<ValidateUserByApiResponseDto>.Success(response);
            }
            catch (ApiException ex)
            {
                BadResponseDto? error = null;

                try
                {
                    error = await ex.GetContentAsAsync<BadResponseDto>();
                }
                catch (Exception)
                {
                    error = new BadResponseDto
                    {
                        Message = "Uventet fejl fra API",
                    };
                }

                
                ApiErrorException apiErrorException = new ApiErrorException(
                    apiErrorMessage: error?.Message,
                    statusCode: (int)ex.StatusCode,
                    original: ex
                );

                return Result<ValidateUserByApiResponseDto>.Error(
                    originalType: null,
                    exception: apiErrorException
                );
            }
            catch (Exception ex)
            {
                return Result<ValidateUserByApiResponseDto>.Error(originalType: null, exception: ex);
            }
        }
    }
}
