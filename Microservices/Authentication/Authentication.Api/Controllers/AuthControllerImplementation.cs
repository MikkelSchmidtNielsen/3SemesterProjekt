using Application.ApplicationDto;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
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
        private readonly IValidateUserHandler _validateUserHandler;

        public AuthControllerImplementation(ICreateUserCommandHandler createUserHandler, IHttpContextAccessor contextAccessor, ICreateOtpCommandHandler createOtpHandler, IValidateUserHandler validateUserHandler)
        {
            _createUserHandler = createUserHandler;
            _contextAccessor = contextAccessor;
            _createOtpHandler = createOtpHandler;
            _validateUserHandler = validateUserHandler;
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

        public async Task<string> ValidateUserAsync(ValidateUserQueryDto body)
        {
            IResult<ValidateUserResponseDto> result = await _validateUserHandler.Handle(body);

            // Handles failures
            if (result.IsSuccess() == false)
            {
                Exception error = result.GetError().Exception!;

                throw error;
            }

            ValidateUserResponseDto dto = result.GetSuccess().OriginalType;

            _contextAccessor.HttpContext!.Response.StatusCode = HttpStatusCode.Created.ToInt();
            return dto.Token;
        }
    }
}
