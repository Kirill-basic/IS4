using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;

namespace IS4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<CustomUser>>();

                var user = new CustomUser("bob") { PhoneNumber="89326092571", Pic="SomeUrl", Picture=new byte[10], Email="bob@mail.ru" };
                userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                userManager.AddClaimAsync(user, new Claim(ClaimTypes.Gender, "GenderFluidHeliSexual"));

            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
