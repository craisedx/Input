using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Input.Business.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Input
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDatabase(host);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
        private static async Task SeedDatabase(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            var seed = scope?.ServiceProvider.GetService<ISeedDatabaseService>();
            if (seed != null)
            {
                await seed.CreateStartRoles();
                seed.CreateStartStatuses();
                await seed.CreateStartAdmin();
            }
        }
    }
}