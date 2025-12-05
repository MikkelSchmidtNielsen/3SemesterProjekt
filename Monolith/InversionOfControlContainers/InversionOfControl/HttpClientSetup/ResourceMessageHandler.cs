using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace InversionOfControlContainers.InversionOfControl.HttpClientSetup
{
    internal class ResourceMessageHandler : DelegatingHandler
    {
        private readonly IConfiguration _config;

        public ResourceMessageHandler(IConfiguration config)
        {
            _config = config;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string apiKey = _config["ApiSettings:ApiKey"]!;

            request.Headers.Add("X-Api-Key", apiKey);

            // request.Headers.Authorization = new AuthenticationHeaderValue("X-Api-Key", apiKey);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
