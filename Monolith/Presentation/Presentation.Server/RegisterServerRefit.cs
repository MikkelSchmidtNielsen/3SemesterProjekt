using Infrastructure.InternalApiCalls.ResourceApi;
using Infrastructure.InternalApiCalls.UserAuthenticationApi;
using Presentation.Shared.Refit;
using Refit;

namespace Presentation.Server
{
	public static class RegisterServerRefit
	{
		public static void RegisterRefit(this IServiceCollection services)
		{
			// Adds Refit Interface by getting it from IHttpClientFactory at runtime
			services
				.AddRefitClient<IUpdateResourceApi>()
				.ConfigureHttpClient((sp, client) =>
				{
					var factory = sp.GetRequiredService<IHttpClientFactory>();
					var http = factory.CreateClient("ServerBaseUrl");
					client.BaseAddress = http.BaseAddress;
				});
		}
	}
}
