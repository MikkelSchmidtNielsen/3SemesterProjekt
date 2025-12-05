using InversionOfControlContainers.InversionOfControl;
using Presentation.Client.Services.Implementation;
using Presentation.Client.Services.Interfaces;
using Presentation.Server;
using Presentation.Server.Components;
using Radzen;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // Add Radzen components
            builder.Services.AddRadzenComponents();
            builder.Services.AddControllers();

            // Register services to IoC
            IocServiceRegistration.RegisterService(builder.Services, builder.Configuration);

            // ServerApis as Refit for Client side
            builder.Services.RegisterRefit();
            builder.Services.AddScoped<IUpdateResourceService, UpdateResourceService>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.MapControllers();

            app.Run();
        }
    }
}
