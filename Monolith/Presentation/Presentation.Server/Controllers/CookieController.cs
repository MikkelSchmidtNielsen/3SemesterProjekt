using Microsoft.AspNetCore.Mvc;

namespace Presentation.Server.Controllers
{
    public class CookieController : Controller
    {
        public void SetCookie(string key, string token)
        {
            CookieOptions options = new CookieOptions();
            DateTime expiryTime = DateTime.UtcNow.AddHours(4);

            Response.Cookies.Append(key, token, expiryTime);
        }
    }
}
