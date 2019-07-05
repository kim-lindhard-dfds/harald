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
            // Handle whatever messages one might wanna sent via Slack when said event is triggered, here.
            _logger.LogDebug($"k8s_namespace_created_and_aws_arn_connected not in use yet. Do something with the event at EventHandlers/K8sNamespaceCreatedAndAwsArnConnectedDomainEventHandler.cs");
        }
    }
}