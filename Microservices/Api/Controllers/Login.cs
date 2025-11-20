using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Login : ControllerBase
	{
		[HttpPost]
		public IActionResult RegisterUser([FromBody] string email)
		{

		}
	}
}
