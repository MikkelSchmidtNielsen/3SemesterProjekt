using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.InversionOfControl.ConfigurationSetup;
using Common.InversionOfControl.HttpClientSetup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.InversionOfControl
{
    public class IocServiceRegistration
    {
        public static void RegisterService(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IConfigurationFactory, ConfigurationFactory>(sp => new ConfigurationFactory(configuration));


            HttpClientModule.RegisterHttpClients(services, configuration);
        }
    }
}
