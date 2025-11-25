using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.Services.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Repository;

namespace InversionOfControlContainers.InversionOfControl
{
    public static class IocServiceRegistration
    {
        public static void RegisterService(IServiceCollection services, IConfiguration configuration)
        {
            //Add DbContext
            string connString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured");

            services.AddDbContext<MySqlServerDbContext>(options =>
            {
                options.UseMySql(connString, ServerVersion.AutoDetect(connString));
            });

            //Add services
            string secretKey = configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is not configured.");

            services.AddScoped<ICreateTokenCommandHandler>(_ => new CreateTokenCommandHandler(secretKey));

            services.AddScoped<ICreateUserCommandHandler, CreateUserCommandHandler>();

            //Add repositories
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
