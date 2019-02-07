using System.Threading.Tasks;
using Harald.Application.Facades;
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

        public void Handle(CapabilityCreatedDomainEvent domainEvent)
        {
            _slackFacade.CreateChannel(domainEvent.CapabilityName);
        }
    }
}
