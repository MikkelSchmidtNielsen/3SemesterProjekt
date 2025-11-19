using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Application.Services.Query;
using Common.ExternalConfig;
using Domain.DomainInterfaces;
using InversionOfControlContainers.InversionOfControl.HttpClientSetup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EntityFramework;
using Persistence.Repository;
using System.ComponentModel.Design;

namespace InversionOfControlContainers.InversionOfControl
{
    public class IocServiceRegistration
    {
        public static void RegisterService(IServiceCollection services, IConfiguration configuration)
        {
            ConfigurationDictionary.CreateDictionary(configuration);

            //Add DbContext
            services.AddDbContext<SqlServerDbContext>();

            //Add services
            services.AddScoped<ICreateResourceService, CreateResourceService>();

            services.AddScoped<IBookingCreateCommand, BookingCreateCommand>();
            services.AddScoped<IGuestCreateCommand, GuestCreateCommand>();
            services.AddScoped<IGuestIdQuery, GuestIdQuery>();
            services.AddScoped<IResourceAllQuery, ResourceAllQuery>();
            services.AddScoped<IResourceIdQuery, ResourceIdQuery>();

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
