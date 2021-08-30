using Constants;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;


namespace MVCClient
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

                    config.ClientId = Clients.Mvc;
                    config.ClientSecret = Secrets.MvcSecret;
                    config.SaveTokens = true;
                    config.ResponseType = "code";

                    config.Scope.Add(Scopes.ApiOneScope);
                    config.Scope.Add(Scopes.ApiTwoScope);
                    config.Scope.Add(Scopes.ApiThreeScope);
                    // config.Scope.Add("openid");
                    // config.Scope.Add("profile");

                    //config.Scope.Add(IdentityServerConstants.LocalApi.ScopeName);
                    config.Scope.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("IsManager", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "Manager");

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