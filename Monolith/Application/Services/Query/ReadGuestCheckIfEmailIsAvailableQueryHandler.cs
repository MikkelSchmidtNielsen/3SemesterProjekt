using Application.ApplicationDto.Query;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
	public class ReadGuestCheckIfEmailIsAvailableQueryHandler : IReadGuestCheckIfEmailIsAvailableQueryHandler
	{
		private readonly IGuestRepository _guestRepository;

		public ReadGuestCheckIfEmailIsAvailableQueryHandler(IGuestRepository guestRepository)
		{
			_guestRepository = guestRepository;
		}

		public async Task<IResult<ReadGuestCheckIfEmailIsAvailableResponseDto>> HandleAsync(ReadGuestCheckIfEmailIsAvailableQueryDto dto)
		{
			// Checks if Email exist in database
			var repoResult = await _guestRepository.CheckIfEmailIsAvailableAsync(dto.Email);

			// If repoResult is not a succes, then returns a new error
			if (repoResult.IsSucces() is false)
			{
				var errorResponseDto = new ReadGuestCheckIfEmailIsAvailableResponseDto() { Email = repoResult.GetError().OriginalType };
				return Result<ReadGuestCheckIfEmailIsAvailableResponseDto>.Error(errorResponseDto, repoResult.GetError().Exception!);
			}

			// Maps the reponse based on the repository reponse 
			var responseDto = new ReadGuestCheckIfEmailIsAvailableResponseDto() { Email = repoResult.GetSuccess().OriginalType };

			// Returns a success
			return Result<ReadGuestCheckIfEmailIsAvailableResponseDto>.Success(responseDto);
		}
	}
}
