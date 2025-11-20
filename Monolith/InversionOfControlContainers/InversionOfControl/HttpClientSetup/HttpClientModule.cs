using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversionOfControlContainers.InversionOfControl.HttpClientSetup
{
    internal class HttpClientModule
    {
        internal static void RegisterHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            // Creates Microsofts IHttpClientFactory
            services.AddHttpClient();

			// Register named clients
			services.AddHttpClient("Authentication", client =>
			{
				client.BaseAddress = new Uri(configuration["ApiBaseUrls:Authentication"]!);
				client.Timeout = TimeSpan.FromSeconds(30);
			});


		}
    }
}
