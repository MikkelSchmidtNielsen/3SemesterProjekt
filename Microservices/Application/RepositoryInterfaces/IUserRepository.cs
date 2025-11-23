using Common.ResultInterfaces;
using Domain;

namespace Application.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<IResult<User>> CreateUserAsync(User user);
    }
}
