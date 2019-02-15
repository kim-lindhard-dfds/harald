using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.EventHandlers
{
    public class SlackCapabilityCreatedDomainEventHandler : IEventHandler<CapabilityCreatedDomainEvent>
    {
        public string EventType => "capabilitycreated";
        public Type EventTypeImplementation => typeof(CapabilityCreatedDomainEvent);
        private readonly ILogger<SlackCapabilityCreatedDomainEventHandler> _logger;
        private readonly ISlackFacade _slackFacade;
        
        public SlackCapabilityCreatedDomainEventHandler(
            ILogger<SlackCapabilityCreatedDomainEventHandler> logger,
            ISlackFacade slackFacade)
        {
            _logger = logger;
            _slackFacade = slackFacade;
        }

        public async Task HandleAsync(CapabilityCreatedDomainEvent domainEvent)
        {
            var createChannelResponse = await _slackFacade.CreateChannel(domainEvent.Data.CapabilityName);
            
            if (createChannelResponse.Ok)
            {
                // TODO: Save Slack channel ID for capability.
                var channelId = createChannelResponse?.Channel?.Id;
                var channelName = createChannelResponse?.Channel?.Name;
                _logger.LogInformation($"Slack channel '{channelName}' for capability '{domainEvent.Data.CapabilityName}' created with ID: {channelId}");
            }
        }
    }
}