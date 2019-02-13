using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Harald.Application.Facades.Slack;
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

        private Task _executingTask;

        public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new Dictionary<string, object>
            {
                {"group.id", "harald-consumer"},
                {"bootstrap.servers", "localhost:9092"},
                {"auto.commit.enable", "false"},
                {"debug", "consumer"},
                //{"log_level", "7" },
            };


            _executingTask = Task.Factory.StartNew(async () =>
                {
                    using (var consumer = new Consumer<string, string>(config, new StringDeserializer(Encoding.UTF8), new StringDeserializer(Encoding.UTF8)))
                    {
                        consumer.Subscribe(new[] {"capability"});
                        consumer.OnPartitionsRevoked += (sender, topicPartitions) => consumer.Unassign();
                        consumer.OnPartitionsAssigned += (sender, topicPartitions) => consumer.Assign(topicPartitions);
                        //consumer.OnLog += Consumer_OnLog;
                        //consumer.OnStatistics += Consumer_OnStatistics;

                        // consume loop
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            if (consumer.Consume(out var msg, 1000))
                            {
                                _logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>> Topic: {Topic} Partition: {Partition}, Offset: {Offset} {Value}", msg.Topic, msg.Partition, msg.Offset, msg.Value);

                                var requiredService = _serviceProvider.GetRequiredService<ISlackFacade>();

                                var response = await requiredService.CreateChannel(msg.Value);

                                var channelId = response.Channel.Id;

                                await requiredService.InviteToChannel("janie@dfds.com", channelId);

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