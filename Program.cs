using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_OP_Web.Data;
using WebAxe.Data;

namespace Test_OP_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
     


            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host).Wait();

            host.Run();
        }

        private static async Task CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<OptionContext>();

                    var userManager = services.GetRequiredService<UserManager<UserAxe>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    DbInitializer.Initialize(context);
                    //await DbInitializer.RoleInitialize(userManager, rolesManager);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
