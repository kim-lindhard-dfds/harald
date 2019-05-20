using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.WebApi.EventHandlers
{
    public class SlackContextAddedToCapabilityDomainEventHandler : IEventHandler<ContextAddedToCapabilityDomainEvent>
    {
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly ISlackFacade _slackFacade;

        public SlackContextAddedToCapabilityDomainEventHandler(ICapabilityRepository capabilityRepository,
            ISlackFacade slackFacade)
        {
            _capabilityRepository = capabilityRepository;
            _slackFacade = slackFacade;
        }

        public async Task HandleAsync(ContextAddedToCapabilityDomainEvent domainEvent)
        {
            var capability = await _capabilityRepository.Get(domainEvent.Data.CapabilityId);

            var message = CreateMessage(domainEvent, capability);

            var hardCodedDedChannelId = "GFYE9B99Q";
            await _slackFacade.SendNotificationToChannel(hardCodedDedChannelId, message);
        }

        public static string CreateMessage(ContextAddedToCapabilityDomainEvent domainEvent, Capability capability)
        {
            var capabilityNameMessage = capability != null
                ? $" capability : \"{capability.Name}\"."
                : " no capability matching the id could be found in Haralds database, this could be symptom of data being out of sync";

            var message = "Context added to capability\n" +
                          $"contextId: \"{domainEvent.Data.ContextId}\" contextName: \"{domainEvent.Data.ContextName}\"\n" +
                          $"capabilityId: \"{domainEvent.Data.CapabilityId}\"" + capabilityNameMessage;


            return message;
        }
    }
}