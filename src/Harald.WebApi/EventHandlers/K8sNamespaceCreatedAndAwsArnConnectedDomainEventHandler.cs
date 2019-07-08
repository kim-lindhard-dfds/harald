using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.EventHandlers
{
    public class K8sNamespaceCreatedAndAwsArnConnectedDomainEventHandler : IEventHandler<K8sNamespaceCreatedAndAwsArnConnectedDomainEvent>
    {
        private readonly ILogger<K8sNamespaceCreatedAndAwsArnConnectedDomainEventHandler> _logger;
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;

        public K8sNamespaceCreatedAndAwsArnConnectedDomainEventHandler(
            ILogger<K8sNamespaceCreatedAndAwsArnConnectedDomainEventHandler> logger,
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
            await _slackFacade.SendNotificationToChannel(capability.SlackChannelId, $"Namespace {domainEvent.Payload.NamespaceName} has been created.");
        }
    }
}