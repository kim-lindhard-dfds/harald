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
            var capability = await _capabilityRepository.Get(domainEvent.Payload.CapabilityId);

            var message = CreateMessage(domainEvent, capability);

            var hardCodedDedChannelId = "GFYE9B99Q";
            await _slackFacade.SendNotificationToChannel(hardCodedDedChannelId, message);
        }

        public static string CreateMessage(ContextAddedToCapabilityDomainEvent domainEvent, Capability capability)
        {
            var capabilityNameMessage = capability != null
                ? $" capability : \"{capability.Name}\"."
                : $" no capability matching the id could be found in Haralds database, this could be symptom of data being out of sync. The name from event is: {domainEvent.Payload.CapabilityName}";
            
            var message = "Context added to capability\n" +
                          $"contextId: \"{domainEvent.Payload.ContextId}\" contextName: \"{domainEvent.Payload.ContextName}\"\n" +
                          $"capabilityId: \"{domainEvent.Payload.CapabilityId}\"" + capabilityNameMessage + "\n" +
                          $"capabilityRootId: {domainEvent.Payload.CapabilityRootId}\n" +
                          $"x-correlationId: {domainEvent.XCorrelationId}, x-sender: {domainEvent.XSender}";


            return message;
        }
    }
}