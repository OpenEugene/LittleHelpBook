using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEugene.Module.LittleHelpBook.Repository;

namespace OpenEugene.Module.LittleHelpBook.Startup
{
    public class LittleHelpBookServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextFactory<LittleHelpBookContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
