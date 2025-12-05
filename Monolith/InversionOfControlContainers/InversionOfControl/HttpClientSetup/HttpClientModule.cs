using Infrastructure.InternalApiCalls.ResourceApi;
using Infrastructure.InternalApiCalls.UserAuthenticationApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
	
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

			services.AddHttpClient("Resource", client =>
			{
				client.BaseAddress = new Uri(configuration["ApiBaseUrls:Resource"]!);
				client.Timeout = TimeSpan.FromSeconds(30);
			});

			services.AddHttpClient("ServerBaseUrl", client =>
			{
				client.BaseAddress = new Uri(configuration["ServerBaseUrl"]!);
				client.Timeout = TimeSpan.FromSeconds(30);
			});

			RegisterRefit(services);
		}

		private static void RegisterRefit(IServiceCollection services)
		{
			// Adds Refit Interface by getting it from IHttpClientFactory at runtime
			services
				.AddRefitClient<IUserAuthenticationApi>()
				.ConfigureHttpClient((sp, client) =>
				{
					var factory = sp.GetRequiredService<IHttpClientFactory>();
					var http = factory.CreateClient("Authentication");
					client.BaseAddress = http.BaseAddress;
				});

			services
				.AddRefitClient<IResourceApi>()
				.ConfigureHttpClient((sp, client) =>
				{
					IHttpClientFactory factory = sp.GetRequiredService<IHttpClientFactory>();
					HttpClient http = factory.CreateClient("Resource");
					client.BaseAddress = http.BaseAddress;
				})
				.AddHttpMessageHandler<ResourceMessageHandler>();
		}
    }
}
