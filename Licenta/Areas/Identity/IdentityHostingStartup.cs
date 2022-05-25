using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Licenta.Areas.Identity.IdentityHostingStartup))]
namespace Licenta.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}