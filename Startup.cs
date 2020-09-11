using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anthill.API.DTO;
using Anthill.API.Filters;
using Anthill.API.Models;
using Anthill.API.Services;
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
            services.AddDbContext<ApplicationDbContent>(options => options.UseSqlServer(this.ConfigurationRoot.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddTransient<EmailService>();
            services.AddCors();

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

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContent>()
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

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller}/{action=Index}/{id?}");
            });


            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                ApplicationDbContent content = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContent>();
                ProjectsDto.Initial(content);
            }
        }
    }
}
