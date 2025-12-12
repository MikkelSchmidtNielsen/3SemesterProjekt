using Common.ExtensionMethods;
using System.Net;

namespace Api.Middleware
{
	public class ApiKeyAuthMiddleware : IMiddleware
	{
		private readonly IConfiguration _configuration;

		public ApiKeyAuthMiddleware(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
			{
				context.Response.StatusCode = HttpStatusCode.Unauthorized.ToInt();
				await context.Response.WriteAsync("API Key missing");
				return;
			}

			var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
			if (!apiKey.Equals(extractedApiKey))
			{
				context.Response.StatusCode = HttpStatusCode.Unauthorized.ToInt();
				await context.Response.WriteAsync("Invalid API Key");
				return;
			}

			await next(context);
		}
	}

	public static class AuthConstants
	{
		public const string ApiKeySectionName = "Authentication:ApiKey";
		public const string ApiKeyHeaderName = "X-Api-Key";
	}
}
