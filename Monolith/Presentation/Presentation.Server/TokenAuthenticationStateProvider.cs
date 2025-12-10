using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Headers;

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
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //string? authCookie = _httpContextAccessor.HttpContext.Request.Cookies["authCookie"];
            string authCookie = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyIiwiZW1haWwiOiJzYXJhaHNvZXJlbnNlbjY0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ikd1ZXN0IiwiZXhwIjoxNzY1Mzc2NzM0fQ.6X8v0nb0yIPNecqokNkaiDiyp0bphO_VQik60_8q5ac";

            AuthenticationState notAuthenticated = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            if (authCookie == null) // THe current user will be marked as not authenticated, if the authCookie can't be found
            {
                NotifyAuthenticationStateChanged(Task.FromResult(notAuthenticated));
                return notAuthenticated;
            }

            try
            {
                JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(authCookie);

                /*Retrieving claims and roles*/
                IEnumerable<Claim> claims = token.Claims;
                ClaimsIdentity identity = new ClaimsIdentity(claims);

                ClaimsPrincipal currentUser = new ClaimsPrincipal(identity);
                AuthenticationState currentUserAuthState = new AuthenticationState(currentUser);
                NotifyAuthenticationStateChanged(Task.FromResult(currentUserAuthState));
                return currentUserAuthState;

            }
            catch (Exception ex) // If the cookie is expired, the user will be marked as not authenticated
            {
                NotifyAuthenticationStateChanged(Task.FromResult(notAuthenticated));
                return notAuthenticated;
            }
        }
    }
}
