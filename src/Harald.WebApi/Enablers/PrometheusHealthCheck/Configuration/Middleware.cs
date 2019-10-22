using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Harald.WebApi.Enablers.PrometheusHealthCheck.Configuration
{
    public static class Middleware
    {
        public static IApplicationBuilder UsePrometheusHealthCheck(
            this IApplicationBuilder app
        )
        {
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = PrometheusHealthProbe.WriteHealthResponseAsync
            });

            return app;
        }
    }
}