using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;

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
            var capability = await _capabilityRepository.Get(domainEvent.Payload.CapabilityId);

            // 1st Message, instant.
            var missingAdsyncTaskTable = SlackContextAddedToCapabilityDomainEventHandler.CreateTaskTable(
                awsAccDone:true, 
                k8sCreatedDone:true, 
                adsyncDone:false
            );
            
            await _slackFacade.SendNotificationToChannel(
                capability.ChannelId, 
                $"Nearly there... time to grab a coffee?\n{missingAdsyncTaskTable}"
                );

            var timeToWait = (60 * 15); // 15 Minutes
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + timeToWait;
            
            // 2nd Message, delayed.
            var allDoneTaskTable = SlackContextAddedToCapabilityDomainEventHandler.CreateTaskTable(
                awsAccDone:true,
                k8sCreatedDone: true, 
                adsyncDone:true
            );
            await _slackFacade.SendDelayedNotificationToChannel(
                capability.ChannelId, 
                $"All done:\n{allDoneTaskTable}", 
                timestamp
            );
        }
    }
}