using Application.ApplicationDto;
using Application.ServiceInterfaces.Command;
using Common.ExtensionMethods;
using Common.ResultInterfaces;
using System.Net;

namespace Authentication.Api.Controllers
{
    public class AuthControllerImplementation : IAuthController
    {
        private readonly ICreateUserCommandHandler _handler;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthControllerImplementation(ICreateUserCommandHandler handler, IHttpContextAccessor contextAccessor)
        {
            _handler = handler;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> RegisterUserAsync(string email)
        {
            IResult<CreateUserResponseDto> result = await _handler.Handle(email);

            // Handles failures
            if (result.IsSuccess() == false)
            {
                Exception error = result.GetError().Exception!;

                throw error;
            }

            CreateUserResponseDto dto = result.GetSuccess().OriginalType;

            _contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.Created.ToInt();
            return dto.Token;
        }
    }
}
