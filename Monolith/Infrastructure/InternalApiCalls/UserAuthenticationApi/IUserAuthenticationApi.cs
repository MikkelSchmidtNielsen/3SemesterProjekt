using Application.ApplicationDto;
using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Application.InfrastructureDto;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.InternalApiCalls.UserAuthenticationApi
{
	public interface IUserAuthenticationApi
	{
		/// <summary>
		/// Registers a user by email. Returns a JWT on success.
		/// </summary>
		/// <param name="email">User email to register</param>
		/// <returns>JWT token as string</returns>
		[Post("/register-user/{email}")]
		Task<string> RegisterUserAsync([AliasAs("email")] string email);

		// Generates an One Time Password and uses the given email as the OTP's key.
		[Put("/request-otp/{email}")]
		Task RequestOtpAsync([AliasAs("email")] string email);

        // Validates user and the given one time password. Returns JWT Token as a string.
        [Get("/validate-user")]
        Task<string> ValidateUserAsync([Body] ValidateUserQueryDto dto);
    }
}
