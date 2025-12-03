using Application.InfrastructureDto;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InfrastructureInterfaces
{
	public interface IUserAuthenticationApiService
	{
		public Task<IResult<CreateUserByApiReponseDto>> RegisterUserAsync(string email);
	}
}
