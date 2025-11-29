using Application.ApplicationDto;
using Common.ResultInterfaces;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateUserCommandHandler
    {
        public Task<IResult<CreateUserResponseDto>> Handle(string email);
    }
}
