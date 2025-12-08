using Common.ResultInterfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<IResult<User>> CreateUserAsync(User user);

        Task<IResult<User>> ReadUserByEmailAsync(string email);

        Task<IResult<User>> UpdateOtpAsync(string email, int otp, DateTime expiryTime);
    }
}
