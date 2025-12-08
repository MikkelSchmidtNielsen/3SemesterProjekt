using Application.Factories;
using Application.InfrastructureInterfaces;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Application.Services.Query;
using Common.ExternalConfig;
using Domain.DomainInterfaces;
using Infrastructure.Email;
using Infrastructure.InternalApiCalls.ResourceApi;
using Infrastructure.InternalApiCalls.UserAuthenticationApi;
using InversionOfControlContainers.InversionOfControl.HttpClientSetup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.EntityFramework;
using Persistence.Repository;

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
            services.AddScoped<ICreateResourceCommand, CreateResourceCommand>();
            services.AddScoped<IBookingCreateCommand, BookingCreateCommand>();
            services.AddScoped<IGuestCreateCommand, GuestCreateCommand>();
            services.AddScoped<IGuestIdQuery, GuestIdQuery>();
            services.AddScoped<IBookingCheckInQuery, BookingCheckInQuery>();
            services.AddScoped<IBookingCheckOutQuery, BookingCheckOutQuery>();
            services.AddScoped<ISendEmail, SendEmailMailKit>();
            services.AddScoped<ICreateBookingByGuestCommandHandler, GuestCreateBookingService>();
			services.AddScoped<IUserAuthenticationApiService, UserAuthenticationApiService>();
            services.AddScoped<IReadGuestByEmailQuery, ReadGuestByEmailQuery>();
            services.AddScoped<IReadGuestCheckIfEmailIsAvailableQueryHandler, ReadGuestCheckIfEmailIsAvailableQueryHandler>();
            services.AddScoped<IResourceApiService, ResourceApiService>();
            services.AddScoped<IReadAllResourcesQuery, ReadAllResourcesQuery>();
            services.AddScoped<IReadResourceByIdQuery, ReadResourceByIdQuery>();
            services.AddScoped<IOtpCreateCommand, OtpCreateCommand>();
            services.AddScoped<IValidateUserService, ValidateUserService>();

			//Add repositories
			services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();

            //Add factories
            services.AddScoped<IBookingFactory, BookingFactory>();
            services.AddScoped<IGuestFactory, GuestFactory>();

			//Add UnitOfWork
			services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Add message handler
            services.AddTransient<ResourceMessageHandler>();

			HttpClientModule.RegisterHttpClients(services, configuration);
        }
    }
}
