using System;
using System.Threading;
using System.Threading.Tasks;
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
            Console.WriteLine($"Starting event consumer.");

            _logger = logger;
            _serviceProvider = serviceProvider;
            // TODO: Inject topics, or introduce registry which can be injected.
            _topics = new[] { "build.capabilities" };
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
                                using (var scope = _serviceProvider.CreateScope())
                                {
                                    _logger.LogInformation($"Received event: Topic: {msg.Topic} Partition: {msg.Partition}, Offset: {msg.Offset} {msg.Value}");

                                    try
                                    {
                                        var eventDispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();
                                        await eventDispatcher.Send(msg.Value);

                                        await consumer.CommitAsync(msg);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex.Message, ex);
                                    }
                                }
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