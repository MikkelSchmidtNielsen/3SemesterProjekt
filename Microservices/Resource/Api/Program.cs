
using Api.Controllers;
using Api.Middleware;
using InversionOfControlContainers.InversionOfControl;

namespace Api
{
    public class Program
    {
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Controllers
			builder.Services.AddControllers();
			builder.Services.AddHttpContextAccessor();

			// Swagger 
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// IoC
			builder.Services.AddScoped<IResourceController, ResourceControllerImplementation>();


			//IocServiceRegistration.RegisterService(builder.Services, builder.Configuration);
			builder.Services.RegisterService(builder.Configuration);

			// ExceptionHandler Middleware
			builder.Services.AddExceptionHandler<ApiExceptionHandler>();
			builder.Services.AddProblemDetails();

			var app = builder.Build();

			app.UseExceptionHandler();
			app.UseHttpsRedirection();

			app.UseMiddleware<ApiKeyAuthMiddleware>();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
