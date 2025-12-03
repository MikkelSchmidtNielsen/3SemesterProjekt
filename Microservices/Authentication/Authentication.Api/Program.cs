using Authentication.Api.Controllers;
using Authentication.Api.Middleware;
using InversionOfControlContainers.InversionOfControl;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            IocServiceRegistration.RegisterService(builder.Services, builder.Configuration);

            // NSwag-generated interface implementation
            builder.Services.AddScoped<IAuthController, AuthControllerImplementation>();

            builder.Services.AddControllers();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    var config = builder.Configuration.GetSection("Jwt");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddExceptionHandler<ApiExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Ensures that db and tables are created
            IocServiceRegistration.EnsureDatabaseCreated(app.Services);

            app.UseExceptionHandler();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
