using Common.ResultInterfaces;
using Domain.ModelsDto;

namespace Domain.DomainInterfaces
{
    public interface IUserFactory
    {
        IResult<User> Create(CreateUserResponseDto dto);
    }
}
