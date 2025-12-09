using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Application.Services.Query;
using Domain.DomainInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Repository;

namespace InversionOfControlContainers.InversionOfControl
{
    public static class IocServiceRegistration
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            // Get connectionstring from config
            string connString = configuration.GetConnectionString("Default") ?? 
                throw new InvalidOperationException("ConnectionStrings:Default is not configured");

            // Add DbContext
            services.AddDbContext<MySqlServerDbContext>(options =>
            {
                options.UseMySql(connString, new MySqlServerVersion(new Version(8, 0, 43)));
            });

            // Add services
            services.AddScoped<IResourceFactory, ResourceFactory>();

            services.AddScoped<ICreateResourceCommandHandler, CreateResourceCommandHandler>();
            services.AddScoped<IReadResourceByIdQueryHandler, ReadResourceByIdQueryHandler>();
            services.AddScoped<IReadResourceWithCriteriaQueryHandler, ReadResourceWithCriteriaQueryHandler>();
            services.AddScoped<IUpdateResourceByIdCommandHandler, UpdateResourceByIdCommandHandler>();

            // Add repositories
            services.AddScoped<IResourceRepository, ResourceRepository>();
        }
    }
}
