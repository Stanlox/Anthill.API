using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anthill.API.Filters;
using Anthill.API.Mapping;
using Anthill.API.Models;
using Anthill.API.Services;
using Anthill.Infastructure.Data;
using Anthill.Infastructure.Interfaces;
using Anthill.Infastructure.Models;
using Anthill.Infastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Anthill.API
{
    /// <summary>
    /// Configures the application and configures the services.
    /// </summary>
    public class Startup
    {
        private IConfigurationRoot ConfigurationRoot { get; }

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hosting)
        {
            this.ConfigurationRoot = new ConfigurationBuilder().SetBasePath(hosting.ContentRootPath).AddJsonFile("appsettings.json").Build();
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfastructure(Configuration);
            services.AddTransient<EmailService>();
            services.AddCors();
            services.AddControllers();
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddControllers(
                config =>
                {

                    config.Filters.Add(new ExceptionFilter());
                });

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller}/{action=Index}/{id?}");
            });

            ApplicationDbContext.CreateAdminAccount(app.ApplicationServices, this.Configuration).Wait();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                ApplicationDbContext content = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                AnthillDbInitialiser.Initial(content);
            }
        }
    }
}
