using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.Infrastructure.Slack;

namespace Harald.WebApi.Application.EventHandlers
{
    public class K8SNamespaceCreatedAndAwsArnConnectedDomainEventHandler : IEventHandler<K8sNamespaceCreatedAndAwsArnConnectedDomainEvent>
    {
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;

        public K8SNamespaceCreatedAndAwsArnConnectedDomainEventHandler(
            ISlackFacade slackFacade,
            ICapabilityRepository capabilityRepository)
        {
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
        }

        public async Task HandleAsync(K8sNamespaceCreatedAndAwsArnConnectedDomainEvent domainEvent)
        {
            var capabilities = await _capabilityRepository.GetById(domainEvent.Payload.CapabilityId);

            // 1st Message, instant.
            var missingAdsyncTaskTable = SlackContextAddedToCapabilityDomainEventHandler.CreateTaskTable(
                awsAccDone:true, 
                k8sCreatedDone:true, 
                adsyncDone:false
            );
            foreach (var capability in capabilities)
            {
                await _slackFacade.SendNotificationToChannel(
                    capability.SlackChannelId.ToString(), 
                    $"Nearly there... time to grab a coffee?\n{missingAdsyncTaskTable}"
                );
            }

            var timeToWait = (60 * 15); // 15 Minutes
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + timeToWait;
            
            // 2nd Message, delayed.
            var allDoneTaskTable = SlackContextAddedToCapabilityDomainEventHandler.CreateTaskTable(
                awsAccDone:true,
                k8sCreatedDone: true, 
                adsyncDone:true
            );
            
            foreach (var capability in capabilities)
            {
                await _slackFacade.SendDelayedNotificationToChannel(
                    capability.SlackChannelId.ToString(), 
                    $"All done:\n{allDoneTaskTable}", 
                    timestamp
                );
            }
        }
    }
}