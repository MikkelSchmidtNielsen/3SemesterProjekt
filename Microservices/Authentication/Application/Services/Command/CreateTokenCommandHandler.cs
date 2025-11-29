using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Command
{
    public class CreateTokenCommandHandler : ICreateTokenCommandHandler
    {
        private readonly string _secretKey;

        public CreateTokenCommandHandler(string secretKey)
        {
            _secretKey = secretKey;
        }

        /// <summary>
        /// Generates a signed JWT for the authenticated user.
        /// </summary>
        public IResult<string> Handle(User user)
        {
            // Define claims
            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ];

            // Creates a security key from our _secretKey and chooses the HmacSha256 algorithm.
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token with the claims, expiration time, and signing credentials
            JwtSecurityToken token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                signingCredentials: credentials
            );

            return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
