sing Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DBR.Application
{
	public class Startup
	{
		/* Configurações da aplicação, injeção de dependências, modo de Build 
		 * (Producao ou Desenvolvimento)*/

		public IConfiguration Configuration { get; }
		public Startup(IHostEnvironment hostingEnvironment)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(hostingEnvironment.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
				.AddEnvironmentVariables();

			if (hostingEnvironment.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(options =>
					{
						options.LoginPath = "/login";
					});

			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//if (env.IsDevelopment())
			//{
				app.UseDeveloperExceptionPage();
			//}
			//app.UseExceptionHandler("/erro/500");
			//app.UseStatusCodePagesWithRedirects("/erro/{0}");
			app.UseHsts();

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Catalogo}/{action=Index}/{id?}");
			});
		}
	}
}
