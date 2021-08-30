using Constants;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace NoSecretMvc
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = "Cookie";
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = "https://localhost:5001/";

                    config.ClientId = Clients.NoSecret;
                    // config.ClientSecret = Secrets.MvcSecret;
                    config.SaveTokens = true;
                    config.ResponseType = "code";

                    config.Scope.Add(Scopes.ApiOneScope);

                    config.Scope.Add("offline_access");
                    config.Scope.Add("email");
                    config.Scope.Add("openid");
                    config.Scope.Add("profile");

                    config.Scope.Add("rc.scope");

                    //adding scope for local api of is4
                    config.Scope.Add(IdentityServerConstants.LocalApi.ScopeName);
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("IsManager", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "Manager");
                });

                config.AddPolicy("HasPhoneNumber", builder =>
                {
                    builder.RequireClaim(ClaimTypes.MobilePhone);
                });
            });

            services.AddHttpClient();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
