using Application.InfrastructureDto;
using Common.ResultInterfaces;

namespace Application.ServiceInterfaces.Query
{
	public interface IUserAuthenticationApiService
	{
		public Task<IResult<CreateUserByApiReponseDto>> RegisterUserAsync(string email);
		public Task RequestOtpAsync(string email);
	}
}
