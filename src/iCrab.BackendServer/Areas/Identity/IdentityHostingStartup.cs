using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(iCrabee.BackendServer.Areas.Identity.IdentityHostingStartup))]
namespace iCrabee.BackendServer.Areas.Identity
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