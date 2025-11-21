using Application.InfrastructureDto;
using Application.InfrastructureInterfaces;
using Common;
using Common.ResultInterfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				// Tries to get the jwt from api
				string jwt = await _userAuthenticationApi.RegisterUserAsync(email);

				// If 201 response return it in form of CreateUserByApiResponseDto
				CreateUserByApiReponseDto response = new CreateUserByApiReponseDto() { JwtToken = jwt };
				return Result<CreateUserByApiReponseDto>.Success(response);
			}
			catch (ApiException ex)
			{
				// If its anything else but a 201 response (409 or 500 reponse code) try gets its value from api call
				var error = await ex.GetContentAsAsync<BadResponseDto>();

				// A custom Exception, so I can get BadReponse error message, status code and original ApiException message all in one exception
				ApiErrorException apiErrorException = new ApiErrorException(
																apiErrorMessage: error?.Message,
																statusCode: (int)ex.StatusCode,
																original: ex);

				return Result<CreateUserByApiReponseDto>.Error(originalType: null, exception: apiErrorException);
			}
			catch (Exception ex)
			{
				// If something breaks which is not from an Api Response
				return Result<CreateUserByApiReponseDto>.Error(originalType: null, exception: ex);
			}
		}
	}
}
