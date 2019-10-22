using Harald.WebApi.Enablers.KafkaMessageConsumer.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Harald.WebApi.Enablers.KafkaMessageConsumer.Configuration
{
    public static class DependencyInjection
    {
        public static void AddKafkaMessageConsumer(this IServiceCollection services)
        {
            services.AddHostedService<KafkaConsumerHostedService>();
            services.AddTransient<KafkaConsumerFactory.KafkaConfiguration>();
            services.AddTransient<KafkaConsumerFactory>();
        }
    }
}