using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Presentation.Server
{
    /* This custom AuthenticationStateProvider is necessary in order to use AuthorizeView in Blazor. */
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

		public TokenAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            ClaimsPrincipal user;

			if (context.User?.Identity != null && context.User.Identity.IsAuthenticated)
			{
				var identity = new ClaimsIdentity(
					context.User.Claims,
					CookieAuthenticationDefaults.AuthenticationScheme);

				user = new ClaimsPrincipal(identity);
			}
			else
			{
				user = new ClaimsPrincipal(new ClaimsIdentity());
			}

			
			return Task.FromResult(new AuthenticationState(user));
		}
    }
}
