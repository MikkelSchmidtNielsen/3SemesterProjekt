using Microsoft.AspNetCore.Mvc;

namespace Presentation.Server.Controllers
{
    public class CookieController : Controller
    {
        public void SetCookie(string token)
        {
            CookieOptions options = new CookieOptions
            {
                HttpOnly = true, // Ensures that JavaScript code can't access this cookie
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            Response.Cookies.Append("auth_cookie", token, options);
        }
    }
}
