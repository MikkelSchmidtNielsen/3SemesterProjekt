using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainInterfaces
{
	public interface IGuestFactory
	{
		IResult<Guest> Create(string firstName, string lastName, int phoneNumber, string email, string country, string language, string address);
	}
}
