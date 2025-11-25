using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InfrastructureDto
{
	public class CreateUserByApiReponseDto
	{
		/// <summary>
		/// JSON Web Token (JWT) til brug ved login/autorisation
		/// </summary>
		public string JwtToken { get; set; }
	}
}
