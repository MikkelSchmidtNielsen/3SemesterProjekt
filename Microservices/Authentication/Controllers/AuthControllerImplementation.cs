using OpenAPITest.Controllers;
using Application.ServiceInterfaces.Command;
using Application.ApplicationDto;
using Common.ResultInterfaces;

namespace Authentication.Controllers
{
    public sealed class AuthControllerImplementation : IAuthController
    {
        ICreateUserCommandHandler _handler;

        public AuthControllerImplementation(ICreateUserCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task<string> RegisterUserAsync(string email)
        {
            IResult<CreateUserResponseDto> result = await _handler.Handle(email);

            if (result.IsSuccess() == false)
            {
                Exception error = result.GetError().Exception!;

                throw new ArgumentException(error.Message, nameof(email));
            }

            CreateUserResponseDto dto = result.GetSuccess().OriginalType;

            return dto.Token;
        }
    }
}
