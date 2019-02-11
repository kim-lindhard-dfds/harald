using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Harald.Application.Facades;
using Harald.Infrastructure.Facades;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Prometheus;

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

            var connectionString = Configuration["HARALD_DATABASE_CONNECTIONSTRING"];

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<HaraldDbContext>((serviceProvider, options) => { options.UseNpgsql(connectionString); });

            services.AddTransient<ICapabilityRepository, CapabilityRepository>();

            services.AddHostedService<MetricHostedService>();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddNpgSql(connectionString, tags: new[] {"backing services", "postgres"})
                ;
            
            services.AddSwaggerDocument();

            services.AddHttpClient<ISlackFacade, SlackFacade>(cfg =>
            {
                var baseUrl = Configuration["SLACK_API_BASE_URL"];
                if (baseUrl != null)
                {
                    cfg.BaseAddress = new Uri(baseUrl);
                }

                var authToken = Configuration["SLACK_API_AUTH_TOKEN"];
                if (authToken != null)
                {
                    cfg.DefaultRequestHeaders
                    .Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {authToken}");
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUi3();
            
            app.UseMvc();

            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = MyPrometheusStuff.WriteResponseAsync
            });
        }
    }

    public class MetricHostedService : IHostedService
    {
        private const string Host = "0.0.0.0";
        private const int Port = 8080;

        private IMetricServer _metricServer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Staring metric server on {Host}:{Port}");

            _metricServer = new KestrelMetricServer(Host, Port).Start();

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using (_metricServer)
            {
                Console.WriteLine("Shutting down metric server");
                await _metricServer.StopAsync();
                Console.WriteLine("Done shutting down metric server");
            }
        }
    }

    public static class MyPrometheusStuff
    {
        private const string HealthCheckLabelName = "healthcheck";

        private static readonly Gauge HealthChecksDuration;
        private static readonly Gauge HealthChecksResult;

        static MyPrometheusStuff()
        {
            HealthChecksResult = Metrics.CreateGauge("healthcheck",
                "Shows raw health check status (0 = Unhealthy, 1 = Degraded, 2 = Healthy)", new GaugeConfiguration
                {
                    LabelNames = new[] {HealthCheckLabelName},
                    SuppressInitialValue = false
                });

            HealthChecksDuration = Metrics.CreateGauge("healthcheck_duration_seconds",
                "Shows duration of the health check execution in seconds",
                new GaugeConfiguration
                {
                    LabelNames = new[] {HealthCheckLabelName},
                    SuppressInitialValue = false
                });
        }

        public static Task WriteResponseAsync(HttpContext httpContext, HealthReport healthReport)
        {
            UpdateMetrics(healthReport);

            httpContext.Response.ContentType = "text/plain";
            return httpContext.Response.WriteAsync(healthReport.Status.ToString());
        }

        private static void UpdateMetrics(HealthReport report)
        {
            foreach (var (key, value) in report.Entries)
            {
                HealthChecksResult.Labels(key).Set((double) value.Status);

                HealthChecksDuration.Labels(key).Set(value.Duration.TotalSeconds);
            }
        }
    }
}
