
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presentation.Server
{
	public class JwtCookieMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (context.Request.Cookies.TryGetValue("authCookie", out var token))
			{
				try
				{
					var handler = new JwtSecurityTokenHandler();
					var jwt = handler.ReadJwtToken(token);
					var identity = new ClaimsIdentity(jwt.Claims, "jwt");
					context.User = new ClaimsPrincipal(identity);
				}
				catch
				{
					context.User = new ClaimsPrincipal(new ClaimsIdentity());
				}
			}
			else
			{
				context.User = new ClaimsPrincipal(new ClaimsIdentity());
			}

			await next(context);
		}
	}
}
