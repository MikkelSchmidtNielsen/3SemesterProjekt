using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Application.Services.Query;
using Domain.DomainInterfaces;
using InversionOfControlContainers.InversionOfControl.HttpClientSetup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EntityFramework;
using Persistence.Repository;

namespace InversionOfControlContainers.InversionOfControl
{
    public class IocServiceRegistration
    {
        public static void RegisterService(IServiceCollection services, IConfiguration configuration)
        {
            //Add DbContext
            services.AddDbContext<SqlServerDbContext>();

            //Add services
            services.AddScoped<IGetAllResourcesService, GetAllResourcesService>();

            //Add repositories
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();

            //Add factories
            services.AddScoped<IBookingFactory, BookingFactory>();
            services.AddScoped<IResourceFactory, ResourceFactory>();
            services.AddScoped<IGuestFactory, GuestFactory>();
        

            HttpClientModule.RegisterHttpClients(services, configuration);
        }
    }
}
