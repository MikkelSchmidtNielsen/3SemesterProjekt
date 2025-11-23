using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.Services.Command;
using Domain.DomainInterfaces;
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
            services.AddDbContext<SqlServerDbContext>();

            //Add services
            services.AddSingleton<ICreateTokenCommandHandler>(
                new CreateTokenCommandHandler(
                    secretKey: "g9V4p2QmL8sT0wZ3D1aH7nK5fR2bE6yJ",
                    lifeTime: TimeSpan.FromHours(1)
                )
            );

            services.AddScoped<ICreateUserCommandHandler, CreateUserCommandHandler>();

            //Add repositories
            services.AddScoped<IUserRepository, UserRepository>();

            //Add factories
            services.AddScoped<IUserFactory, UserFactory>();
        }
    }
}
