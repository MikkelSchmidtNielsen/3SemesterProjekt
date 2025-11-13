using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.Services.Command;
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
            services.AddScoped<IBookingCreateCommand, BookingCreateCommand>();

            //Add repositories
            services.AddTransient<IBookingRepository, BookingRepository>();
        

            HttpClientModule.RegisterHttpClients(services, configuration);
        }
    }
}
