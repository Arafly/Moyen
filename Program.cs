using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moyen.Persistence.Contexts;

namespace Moyen
{
    public class Program {
        public static void Main (string[] args) {
            var host = CreateWebHostBuilder (args).Build ();

            using (var scope = host.Services.CreateScope ()) {
                var services = scope.ServiceProvider;
                try

                {
                    var context = services.GetRequiredService<MoyenContext> ();
                    context.Database.Migrate ();
                    Seed.SeedData (context);
                } catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>> ();
                    logger.LogError (ex, "An error occured during migration");
                }
            }

            host.Run ();
        }

        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseStartup<Startup> ();
    }
}
