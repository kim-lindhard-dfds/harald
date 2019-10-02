using Microsoft.Extensions.DependencyInjection;

namespace Harald.WebApi.Features.Metrics.Configuration
{
    public static class DependencyInjection
    {
        public static void AddMetricsDependencies(this IServiceCollection services)
        {
            services.AddHostedService<MetricHostedService>();
        }
    }
}