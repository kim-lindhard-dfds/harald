using Harald.WebApi.Features.KafkaMessageConsumer.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Harald.WebApi.Features.KafkaMessageConsumer.Configuration
{
    public static class DependencyInjection
    {
        public static void AddKafkaMessageConsumerDependencies(this IServiceCollection services)
        {
            services.AddHostedService<KafkaConsumerHostedService>();
            services.AddTransient<KafkaConsumerFactory.KafkaConfiguration>();
            services.AddTransient<KafkaConsumerFactory>();
        }
    }
}