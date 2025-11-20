using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainInterfaces
{
	public interface IGuestFactory
	{
		IResult<Guest> Create(GuestCreateUserDomainDto guestCreateUserDomainDto);
	}
}
