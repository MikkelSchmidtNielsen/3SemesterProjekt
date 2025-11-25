using Application.ApplicationDto.Command;
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
		public IResult<Guest> Create(CreatedGuestDto dto)
		{
			Guest guest = new Guest(dto.FirstName, dto.LastName, dto.PhoneNumber, dto.Email, dto.Country, dto.Language, dto.Address);

			return Result<Guest>.Success(guest);
		}
	}
}
