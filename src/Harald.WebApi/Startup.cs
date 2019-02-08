using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Harald.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<HaraldDbContext>((serviceProvider, options) =>
                {
                    var connectionString = Configuration["HARALD_DATABASE_CONNECTIONSTRING"];
                    options.UseNpgsql(connectionString);
                });

            services.AddTransient<ICapabilityRepository, CapabilityRepository>();

            services.AddHealthChecks().Add(new HealthCheckRegistration("DatabaseAlive", sp => new DatabaseHealthCheck(sp.GetRequiredService<ICapabilityRepository>()), HealthStatus.Unhealthy, Enumerable.Empty<string>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseHealthChecks("/healthz");
        }
    }

    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ICapabilityRepository _capabilityRepository;

        public DatabaseHealthCheck(ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _capabilityRepository.Get(Guid.Empty);

                return HealthCheckResult.Healthy();
            }
            catch 
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}