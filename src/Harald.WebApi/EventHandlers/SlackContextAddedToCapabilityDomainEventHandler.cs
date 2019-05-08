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
            
            string message;
            if (capability != null)
            {
                message =
                    $"Context: \"{domainEvent.Data.ContextName}\" have been added to capability : \"{capability.Name}\".";
            }
            else
            {
                message =
                    $"Context: \"{domainEvent.Data.ContextName}\" have been added to capabilityId : \"{domainEvent.Data.CapabilityId.ToString()}\".\n" +
                    "No capability matching the id could be found in Haralds database, this could be symptom of data being out of sync";
            }

            var hardCodedDedChannelId = "GFYE9B99Q";
            await _slackFacade.SendNotificationToChannel(hardCodedDedChannelId, message);
        }
    }
}