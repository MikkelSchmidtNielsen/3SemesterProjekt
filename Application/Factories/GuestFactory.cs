using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
	public class GuestFactory : IGuestFactory
	{
		public IResult<Guest> Create(string firstName, string lastName, int phoneNumber, string email, string country, string language, string address)
		{
			Guest guest = new Guest(firstName, lastName, phoneNumber, email, country, language, address);

			return Result<Guest>.Success(guest);
		}
	}
}
