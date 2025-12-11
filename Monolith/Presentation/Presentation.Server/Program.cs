using InversionOfControlContainers.InversionOfControl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
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

            //Authentication and authorization

            builder.Services.AddAuthentication(options =>
			                                {
				                                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				                                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			                                })
											  .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                                              {
                                                  options.Cookie.Name = "authCookie";
                                                  options.Cookie.HttpOnly = true;
                                                  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                                                  options.Events.OnRedirectToLogin = ctx =>
                                                  {
                                                      ctx.Response.Redirect("/not-allowed");
                                                      return Task.CompletedTask;
                                                  };

                                                  options.Events.OnRedirectToAccessDenied = ctx =>
                                                  {
                                                      ctx.Response.StatusCode = 401;
													  ctx.Response.Redirect("/not-allowed");
													  return Task.CompletedTask;
                                                  };
                                              });

            builder.Services.AddAuthenticationCore();
            //builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
            builder.Services.AddScoped<JwtCookieMiddleware>();

            // Register services to IoC
            IocServiceRegistration.RegisterService(builder.Services, builder.Configuration);

            // ServerApis as Refit for Client side
            builder.Services.RegisterRefit();
            builder.Services.AddScoped<IUpdateResourceService, UpdateResourceService>();
            builder.Services.AddHttpContextAccessor();

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

            app.UseMiddleware<JwtCookieMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.MapControllers();

            app.Run();
        }
    }
}
