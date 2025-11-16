using Common.ResultInterfaces;
using Domain.Models;

namespace Domain.DomainInterfaces
{
	public interface IGuestFactory
	{
		IResult<Guest> Create(string firstName, string lastName, int phoneNumber, string email, string country, string language, string address);
    }
}
