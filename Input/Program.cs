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
        // "DefaultConnection": "Server=WIN-FI5PDLPQ4DV\\DV;Database=InputDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
        //"DefaultConnection": "Server=20.25.120.116;Database=InputDb;User Id=sa;Password=1M3kh4***d3;"

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