
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
				//var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZW1haWwiOiJtaWtrZWxAcm9zZW5kYWhsbGFyc2VuLmRrIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiR3Vlc3QiLCJleHAiOjE3NjUzNzc3NjZ9.XWPlva_6fzUhMs-1GrscCaysLUGB4zX2qK5AHp-16Uc";
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
