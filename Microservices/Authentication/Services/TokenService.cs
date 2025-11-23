using OpenAPITest.Controllers;
using Application.ServiceInterfaces.Command;

namespace Authentication.Services
{
    public sealed class TokenService : IUserControllerController
    {
        ICreateUserCommandHandler _handler;

        public TokenService(ICreateUserCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task<string> RegisterUserAsync(string email)
        {
            var result = await _handler.Handle(email);

            return result.GetSuccess().OriginalType.Token;
        }
    }
}
