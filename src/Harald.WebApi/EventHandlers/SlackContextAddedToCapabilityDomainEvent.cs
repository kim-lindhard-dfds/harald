using System;
using System.Net.Http;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.WebApi.EventHandlers
{
    public class SlackContextAddedToCapabilityDomainEvent : IEventHandler<ContextAddedToCapabilityDomainEvent>
    {
        private readonly HttpClient _client;

        private readonly ICapabilityRepository _capabilityRepository;
        private readonly ISlackFacade _slackFacade;

        public SlackContextAddedToCapabilityDomainEvent(ICapabilityRepository capabilityRepository, ISlackFacade slackFacade)
        {
            _capabilityRepository = capabilityRepository;
            _slackFacade = slackFacade;
        }

        public async Task HandleAsync(ContextAddedToCapabilityDomainEvent domainEvent)
        {
            var capability = await _capabilityRepository.Get(domainEvent.Data.CapabilityId);

            var capabilityName = capability != null ? 
                capability.Name : 
                domainEvent.Data.CapabilityId.ToString();


            var hardCodedDedChannelId = "GFYE9B99Q";

            var message = $"context: \"{domainEvent.Data.ContextName}\" have been added to capability : \"{capabilityName}\". Have  a nice day";

            _slackFacade.SendNotificationToChannel(hardCodedDedChannelId, message);
        }
    }
}