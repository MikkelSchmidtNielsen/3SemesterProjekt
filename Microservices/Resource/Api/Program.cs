
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
			IocServiceRegistration.RegisterService(builder.Services, builder.Configuration);

			// ExceptionHandler Middleware
			builder.Services.AddExceptionHandler<ApiExceptionHandler>();
			builder.Services.AddProblemDetails();

			var app = builder.Build();

			// Ensure DB created. Was not need in Home Test
			//IocServiceRegistration.EnsureDatabaseCreated(app.Services);

			// Middleware if Swagger for Dev Only
			if (app.Environment.IsDevelopment())
			{

			}

			// Set for all uses, so I can use it via Docker
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();

			app.UseExceptionHandler();

			app.Run();
		}
	}
}
