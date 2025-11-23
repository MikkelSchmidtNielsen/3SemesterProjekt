using Common;
using Common.ResultInterfaces;
using Domain;
using Domain.DomainInterfaces;
using Domain.ModelsDto;

namespace Application.Factories
{
    public class UserFactory : IUserFactory
    {
        public IResult<User> Create(CreateUserResponseDto dto)
        {
            User user = new User(dto.Email);

            return Result<User>.Success(user);
        }
    }
}
