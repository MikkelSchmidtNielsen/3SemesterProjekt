using Application.ServiceInterfaces.Command;
using Application.ApplicationDto;
using Common.ResultInterfaces;
using Common.Exceptions;

namespace Authentication.Api.Controllers
{
    public class AuthControllerImplementation : IAuthController
    {
        ICreateUserCommandHandler _handler;

        public AuthControllerImplementation(ICreateUserCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task<string> RegisterUserAsync(string email)
        {
            IResult<CreateUserResponseDto> result = await _handler.HandleAsync(email);

            // Handles failures
            if (result.IsSuccess() == false)
            {
                Exception error = result.GetError().Exception!;

                throw error;
            }

            CreateUserResponseDto dto = result.GetSuccess().OriginalType;

            return dto.Token;
        }
    }
}
