using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public class ConsumerHostedService : IHostedService
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ILogger<ConsumerHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly string[] _topics;

        private Task _executingTask;

        public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            // TODO: Inject topics, or introduce registry which can be injected.
            _topics = new[] { "capability" };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = Task.Factory.StartNew(async () =>
                {
                    var consumerFactory = _serviceProvider.GetRequiredService<KafkaConsumerFactory>();

                    using (var consumer = consumerFactory.Create())
                    {
                        consumer.Subscribe(_topics);
                        consumer.OnPartitionsRevoked += (sender, topicPartitions) => consumer.Unassign();
                        consumer.OnPartitionsAssigned += (sender, topicPartitions) => consumer.Assign(topicPartitions);

                        // consume loop
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            if (consumer.Consume(out var msg, 1000))
                            {
                                _logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>> Topic: {Topic} Partition: {Partition}, Offset: {Offset} {Value}", msg.Topic, msg.Partition, msg.Offset, msg.Value);

                                var eventDispatcher = _serviceProvider.GetRequiredService<IEventDispatcher>();
                                await eventDispatcher.Send(msg.Value);

                                await consumer.CommitAsync(msg);
                            }
                        }
                    }
                }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        _logger.LogError(task.Exception, "Event loop crashed");
                    }
                }, cancellationToken);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
            }

            _cancellationTokenSource.Dispose();
        }
    }
}