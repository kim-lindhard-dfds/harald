using System.Threading.Tasks;
using Harald.Application.Facades.Slack;
using Harald.Domain.Capability.Events;

namespace Harald.Application.EventHandlers
{
    public class SlackCapabilityCreatedDomainEventHandler : IEventHandler<CapabilityCreatedDomainEvent>
    {
        private readonly ISlackFacade _slackFacade;

        public SlackCapabilityCreatedDomainEventHandler(ISlackFacade slackFacade)
        {
            _slackFacade = slackFacade;
        }

        public async Task HandleAsync(CapabilityCreatedDomainEvent domainEvent)
        {
            var createChannelResponse = await _slackFacade.CreateChannel(domainEvent.CapabilityName);

            if (createChannelResponse.Ok)
            {
                // TODO: Save Slack channel ID for capability.
                var channelId = createChannelResponse?.Channel?.Id;
            }
        }
    }
}
