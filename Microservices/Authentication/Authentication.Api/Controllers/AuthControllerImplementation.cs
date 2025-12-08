using Application.ApplicationDto;
using Application.ServiceInterfaces.Command;
using Common.ExtensionMethods;
using Common.ResultInterfaces;
using System.Net;

namespace Authentication.Api.Controllers
{
    public class AuthControllerImplementation : IAuthController
    {
        private readonly ICreateUserCommandHandler _createUserHandler;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICreateOtpCommandHandler _createOtpHandler;

        public AuthControllerImplementation(ICreateUserCommandHandler createUserHandler, IHttpContextAccessor contextAccessor, ICreateOtpCommandHandler createOtpHandler)
        {
            _createUserHandler = createUserHandler;
            _contextAccessor = contextAccessor;
            _createOtpHandler = createOtpHandler;
        }

        public async Task<string> RegisterUserAsync(string email)
        {
            // Creates a user and assigns a token to it
            IResult<CreateUserResponseDto> result = await _createUserHandler.HandleAsync(email);

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

        public Task RequestOtpAsync(string email)
        {
            return _createOtpHandler.Handle(email);
        }
    }
}
