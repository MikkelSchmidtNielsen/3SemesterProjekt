
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presentation.Server
{
	public class JwtCookieMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (true)
			{
				try
				{
					var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1IiwiZW1haWwiOiJUaGVudWtlcjk5NTFAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiR3Vlc3QiLCJleHAiOjE3Njc5Nzc4ODd9.OMSEs6Iyiplq8LBK9E5p_nUyXc1cRlLFP33myJcucO8";

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
