using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Command
{
    public class CreateTokenCommandHandler : ICreateTokenCommandHandler
    {
        private readonly string _secretKey;
        private readonly TimeSpan _lifeTime;

        public CreateTokenCommandHandler(string secretKey, TimeSpan lifeTime)
        {
            _secretKey = secretKey;
            _lifeTime = lifeTime;
        }

        public async Task<IResult<string>> Handle(string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(_lifeTime),
                signingCredentials: creds
            );

            return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
