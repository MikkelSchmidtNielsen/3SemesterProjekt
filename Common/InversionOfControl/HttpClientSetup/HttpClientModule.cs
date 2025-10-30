using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.InversionOfControl.HttpClientSetup
{
    internal class HttpClientModule
    {
        internal static void RegisterHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            // Creates Microsofts IHttpClientFactory
            services.AddHttpClient();

            // Register named clients

        }
    }
}
