using IdentityServer4.Services;
using IS4.Data;
using IS4.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace IS4
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseInMemoryDatabase("MEMORY");
            });

            // AddIdentity registers the services
            services.AddIdentity<CustomUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
            });

            services.AddIdentityServer(config => { config.UserInteraction.LoginUrl = "/Auth/Login"; })
                .AddAspNetIdentity<CustomUser>()
                .AddInMemoryClients(Configuration.GetClients())
                .AddInMemoryApiResources(Configuration.GetApiResources())
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryApiScopes(Configuration.GetApiScopes())
                .AddDeveloperSigningCredential();

            services.AddLocalApiAuthentication();

            services.AddTransient<IProfileService, CustomProfileService>();

            var emailConfig = _config.GetSection("MyEmail").Get<MailKitOptions>();

            services.AddMailKit(config => config.UseMailKit(_config.GetSection("MyEmail").Get<MailKitOptions>()));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
