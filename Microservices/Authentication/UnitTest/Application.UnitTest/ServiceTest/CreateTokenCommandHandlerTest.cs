using Application.Services.Command;
using Common.ResultInterfaces;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class CreateTokenCommandHandlerTest
    {
        [Fact]
        public void Handle_ReturnsJwt_WithCorrectClaims()
        {
            // Arrange
            string email = "test@system.dk";
            string secretKey = "g9V4p2QmL8sT0wZ3D1aH7nK5fR2bE6yJ";
            CreateTokenCommandHandler sut = new CreateTokenCommandHandler(secretKey);

            User user = new User(email);

            // Act
            IResult<string> result = sut.Handle(user);

            // Assert
            Assert.True(result.IsSuccess());

            string jwt = result.GetSuccess().OriginalType;
            Assert.False(string.IsNullOrWhiteSpace(jwt));

            // Validate the token using same key
            TokenValidationParameters validationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = handler.ValidateToken(jwt, validationParams, out SecurityToken validatedToken);

            JwtSecurityToken token = (JwtSecurityToken)validatedToken;

            // Check claims
            Assert.Equal(user.Id.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
        }
    }
}
