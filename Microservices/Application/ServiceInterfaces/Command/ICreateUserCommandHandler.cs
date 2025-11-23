using Common.ResultInterfaces;
using Domain.ModelsDto;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateUserCommandHandler
    {
        public Task<IResult<CreateUserResponseDto>> Handle(string input);
    }
}
