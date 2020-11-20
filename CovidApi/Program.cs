using System.Threading.Tasks;
using CovidApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CovidApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var databaseRepo = services.GetRequiredService<IDatabaseRepository>();
                await databaseRepo.EnsureMigratedAsync();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json");
                    config.AddSystemsManager("/AppSettings/CodeLifterIO/CovidApi/Development");
                })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.ConfigureKestrel(so => { });
                   webBuilder.UseStartup<Startup>();
               });
    }
}
