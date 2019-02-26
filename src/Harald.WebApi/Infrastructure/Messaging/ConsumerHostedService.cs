﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly KafkaConsumerFactory _consumerFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly DomainEventRegistry _eventRegistry;

        private Task _executingTask;

        public ConsumerHostedService(
            ILogger<ConsumerHostedService> logger,
            IServiceProvider serviceProvider,
            KafkaConsumerFactory kafkaConsumerFactory,
            DomainEventRegistry domainEventRegistry)
        {
            Console.WriteLine($"Starting event consumer.");

            _logger = logger;
            _consumerFactory = kafkaConsumerFactory;
            _serviceProvider = serviceProvider;
            _eventRegistry = domainEventRegistry;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = Task.Factory.StartNew(async () =>
                {
                    using (var consumer = _consumerFactory.Create())
                    {
                        var topics = _eventRegistry.GetAllTopics();

                        _logger.LogInformation($"Event consumer started. Listening to topics: {string.Join(",", topics)}");
                                                
                        consumer.Subscribe(topics);
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
                                        await eventDispatcher.Send(msg.Value, scope);

                                        await consumer.CommitAsync(msg);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError($"Error consuming event. Exception message: {ex.Message}", ex);
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