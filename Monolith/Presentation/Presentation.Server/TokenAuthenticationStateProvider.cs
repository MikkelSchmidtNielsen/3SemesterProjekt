using Microsoft.AspNetCore.Components.Authorization;

namespace Presentation.Server
{
    /* This custom AuthenticationStateProvider is necessary in order to use AuthorizeView in Blazor. */
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        /* IHttpContextAccessorr is used to access data from an HttpOnly Cookie. In this case, it's authCookie from CookieController.cs */
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies["authCookie"];
        }
    }
}
