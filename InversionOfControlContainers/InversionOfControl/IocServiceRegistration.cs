using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
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
            services.AddScoped<IGuestCreateBookingService, GuestCreateBookingService>();

            //Add repositories
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();

            //Add factories
            services.AddScoped<IBookingFactory, BookingFactory>();
            services.AddScoped<IResourceFactory, ResourceFactory>();
        

            HttpClientModule.RegisterHttpClients(services, configuration);
        }
    }
}
