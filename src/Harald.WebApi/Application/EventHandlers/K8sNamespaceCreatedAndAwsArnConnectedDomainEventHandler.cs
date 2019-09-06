using System;
using System.Threading.Tasks;
using Harald.WebApi.Application.EventHandlers;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.EventHandlers
{
    public class K8SNamespaceCreatedAndAwsArnConnectedDomainEventHandler : IEventHandler<K8sNamespaceCreatedAndAwsArnConnectedDomainEvent>
    {
        private readonly ILogger<K8SNamespaceCreatedAndAwsArnConnectedDomainEventHandler> _logger;
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;

        public K8SNamespaceCreatedAndAwsArnConnectedDomainEventHandler(
            ILogger<K8SNamespaceCreatedAndAwsArnConnectedDomainEventHandler> logger,
            ISlackFacade slackFacade,
            ICapabilityRepository capabilityRepository)
        {
            _logger = logger;
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
        }

        public async Task HandleAsync(K8sNamespaceCreatedAndAwsArnConnectedDomainEvent domainEvent)
        {
            var capability = await _capabilityRepository.Get(domainEvent.Payload.CapabilityId);
            
            // 1st Message, instant.
            await _slackFacade.SendNotificationToChannel(capability.SlackChannelId, $"Nearly there... time to grab a coffee?\n{SlackContextAddedToCapabilityDomainEventHandler.CreateTaskTable(true, true, false)}");

            var timeToWait = (60 * 15); // 15 Minutes
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + timeToWait;
            // 2nd Message, delayed.
            await _slackFacade.SendDelayedNotificationToChannel(capability.SlackChannelId, $"All done:\n{SlackContextAddedToCapabilityDomainEventHandler.CreateTaskTable(true, true, true)}", timestamp);
        }
    }
}