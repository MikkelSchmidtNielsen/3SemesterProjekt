using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
	public class GuestFactory : IGuestFactory
	{

		private readonly IGuestRepository _guestRepository;

		public GuestFactory(IGuestRepository guestRepository)
		{
			_guestRepository = guestRepository;
		}

		public async Task<IResult<Guest>> CreateAsync(CreatedGuestDto dto)
		{
			var repoResult = await _guestRepository.CheckIfEmailIsAvailable(dto.Email!);

			if (repoResult.IsSucces() is false)
			{
				return Result<Guest>.Error(null, new Exception("Email already exist"));
			}

			Guest guest = new Guest(dto.FirstName, dto.LastName, dto.PhoneNumber, dto.Email, dto.Country, dto.Language, dto.Address);
			return Result<Guest>.Success(guest);
		}
	}
}
