using Harald.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Harald.Tests
{
    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) : base(configuration, hostingEnvironment)
        {

        }

        protected override void AddRepositories(IServiceCollection services)
        {
            //Dont add any repos. They will be injected using the existing WithServices logic.
        }

        protected override void AddMetricServices(IServiceCollection services)
        {
            //Do nothing to avoid loading metric server.
        }

        protected override void AddSlackServices(IServiceCollection services)
        {
            //Do nothing to avoid loading metric server.
        }
    }
}
