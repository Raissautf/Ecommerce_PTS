using DBR.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace DBR.API
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IHostEnvironment hostEnvironment)
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(hostEnvironment.ContentRootPath)
			.AddJsonFile("appsettings.json", true, true)
			.AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
			.AddEnvironmentVariables();

			if (hostEnvironment.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			/*Realiza a configuração do banco de dados, no caso está sendo usado o SQL Server, a configuração é informada
			 no arquivo appsettings.development.json. como caminho do banco, nome, login e senha (se necessário)*/
			services.AddDbContext<ApiContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddControllers();

			services.AddCors(options =>
			{
				options.AddPolicy("Total",
								 builder =>
						 builder
							 .AllowAnyOrigin()
							 .AllowAnyMethod()
							 .AllowAnyHeader());
			});

			//Documentação Swagger, usado para documentar a API
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "Ecommerce Raissa",
					Description = "Esta API faz parte do Ecommerce Raissa",
					Contact = new OpenApiContact() { Name = "Raissa", Email = "raissamacedo7@gmail.com" },
					License = new OpenApiLicense() { Name = "PTS", Url = new Uri("https://opensource.org/licenses/MIT") }
				});
			
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "Insira o token JWT desta maneira: Bearer {seu token}",
					Name = "Authorization",
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey
				});
			
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] {}
					}
				});
			});
			
			services.AddScoped<ApiContext>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("Total");

			//app.UseAuthConfiguration();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

		}
	}
}
