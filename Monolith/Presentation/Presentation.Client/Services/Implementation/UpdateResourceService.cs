using Common;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Presentation.Client.Services.Interfaces;
using Presentation.Shared.Models;
using Presentation.Shared.Refit;
using Refit;
using System.Runtime.CompilerServices;

namespace Presentation.Client.Services.Implementation
{
	public class UpdateResourceService : IUpdateResourceService
	{
		private readonly IUpdateResourceApi _api;

		private UpdateResourceService() // Used for Unittest
		{
		}

		public UpdateResourceService(IUpdateResourceApi api)
		{
			_api = api;
		}


		public async Task<IResult<IEnumerable<UpdateResourceModel>>> GetAllResourcesAsync()
		{
			try
			{
				IEnumerable<UpdateResourceModel> result = await _api.GetAllResources();

				return Result<IEnumerable<UpdateResourceModel>>.Success(result);
			}
			catch (ApiException ex)
			{
				BadResponseDto? error = null;

				// If its anything else but a 201 response (404, 409 or 500 reponse code) try gets its value from api call
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

				// A custom Exception, so we can get error message, status code and original ApiException message all in one exception
				ApiErrorException apiErrorException = new ApiErrorException(
					apiErrorMessage: error?.Message,
					statusCode: (int)ex.StatusCode,
					original: ex
				);

				return Result<IEnumerable<UpdateResourceModel>>.Error(
					originalType: Array.Empty<UpdateResourceModel>(),
					exception: apiErrorException
				);
			}
			catch (Exception ex)
			{
				return Result<IEnumerable<UpdateResourceModel>>.Error(
					originalType: Array.Empty<UpdateResourceModel>(),
					exception: ex
				);
			}
		}

		public async Task<IResult<UpdateResourceModel>> UpdateResourceAsync(UpdateResourceModel resource)
		{
			try
			{
				UpdateResourceModel result = await _api.UpdateResource(resource.Id, resource);

				return Result<UpdateResourceModel>.Success(result);
			}
			catch (ApiException ex)
			{
				BadResponseDto? error = null;

				// If its anything else but a 201 response (404, 409 or 500 reponse code) try gets its value from api call
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

				// A custom Exception, so we can get error message, status code and original ApiException message all in one exception
				ApiErrorException apiErrorException = new ApiErrorException(
					apiErrorMessage: error?.Message,
					statusCode: (int)ex.StatusCode,
					original: ex
				);

				return Result<UpdateResourceModel>.Error(
					originalType: null,
					exception: apiErrorException
				);
			}
			catch (Exception ex)
			{
				return Result<UpdateResourceModel>.Error(
					originalType: null,
					exception: ex
				);
			}
		
		}
	}
}
