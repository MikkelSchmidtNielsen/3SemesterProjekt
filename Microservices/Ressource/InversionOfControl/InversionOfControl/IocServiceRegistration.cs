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
            // Get connectionstring from config
            string connString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured");

            // Add DbContext
            services.AddDbContext<MySqlServerDbContext>(options =>
            {
                options.UseMySql(connString, new MySqlServerVersion(new Version(8, 0, 43)));
            });

            // Get secretKey from config
            string secretKey = configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is not configured.");

            // Add services
            //services.AddScoped<ICreateTokenCommandHandler>(_ => new CreateTokenCommandHandler(secretKey));

            //services.AddScoped<ICreateUserCommandHandler, CreateUserCommandHandler>();

            // Add repositories
            services.AddScoped<IResourceRepository, ResourceRepository>();
        }

        /// <summary>
        /// Ensures the database and the user table exist before the application starts.
        /// Handles the "race condition" where the application might start faster than the database is ready.
        /// </summary>
        public static void EnsureDatabaseCreated(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MySqlServerDbContext>();

                // Ensures that the program startup is blocked until the database is reachable and the user table is confirmed
                context.Database.EnsureCreated();
            }
        }
    }
}
