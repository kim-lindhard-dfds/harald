using Microsoft.Extensions.DependencyInjection;

namespace Harald.WebApi.Enablers.Metrics.Configuration
{
    public static class DependencyInjection
    {
        public static void AddMetrics(this IServiceCollection services)
        {
            services.AddHostedService<MetricHostedService>();
        }
    }
}