namespace Presentation.Server
{
	public class CookieService
	{

		private readonly IHttpContextAccessor _httpContextAccessor;

		public CookieService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public void SetAuthCookie(string token)
		{
			var context = _httpContextAccessor.HttpContext;

			var options = new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				Expires = DateTime.UtcNow.AddDays(1),
				SameSite = SameSiteMode.Strict
			};

			context.Response.Cookies.Append("authCookie", token, options);
		}
	}
}
