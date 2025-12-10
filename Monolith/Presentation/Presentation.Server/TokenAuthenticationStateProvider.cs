using Microsoft.AspNetCore.Components.Authorization;

namespace Presentation.Server
{
    /* This custom AuthenticationStateProvider is necessary in order to use AuthorizeView in Blazor. */
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        /* IHttpContextAccessorr is used to access data from the Httponly Cookie. */
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
