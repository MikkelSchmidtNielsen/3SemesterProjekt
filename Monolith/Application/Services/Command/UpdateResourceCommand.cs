using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
	public class UpdateResourceCommand : IUpdateResourceCommand
	{
		private readonly IResourceApiService _apiService;

		private UpdateResourceCommand() // Used for UnitTest
		{
		}

		public UpdateResourceCommand(IResourceApiService apiService)
		{
			_apiService = apiService;
		}
		public async Task<IResult<UpdateResourceResponseDto>> HandleAsync(UpdateResourceCommandDto command)
		{
			IResult<UpdateResourceByApiResponseDto> apiResponse = await _apiService.UpdateAsync(command);

			if (apiResponse.IsSucces())
			{
				return Result<UpdateResourceResponseDto>.Success(Mapper.Map<UpdateResourceResponseDto>(apiResponse.GetSuccess().OriginalType));
			}
			else
			{
				return Result<UpdateResourceResponseDto>.Error(null, apiResponse.GetError().Exception!);
			}
		}
	}
}
