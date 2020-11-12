using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CovidApi.Areas.Identity.IdentityHostingStartup))]
namespace CovidApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}