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
        private readonly HttpClient _httpClient;

        public TokenAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient("ServerBaseUrl");
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? authCookie = _httpContextAccessor.HttpContext.Request.Cookies["authCookie"];

            AuthenticationState notAuthenticated = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _httpClient.DefaultRequestHeaders.Authorization = null;


            if (authCookie == null) // THe current user will be marked as not authenticated, if the authCookie can't be found
            {
                NotifyAuthenticationStateChanged(Task.FromResult(notAuthenticated));
                return notAuthenticated;
            }

            try
            {
                JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(authCookie);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString().Replace("\"", ""));

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
