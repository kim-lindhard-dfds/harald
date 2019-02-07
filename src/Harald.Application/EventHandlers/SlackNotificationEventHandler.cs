using System.Threading.Tasks;
using Harald.Application.Facades;
using Harald.Domain.Capability.Events;

namespace Harald.Application.EventHandlers
{
    public class SlackNotificationEventHandler : IEventHandler<SendNotificationDomainEvent>
    {
        private readonly ISlackFacade _slackFacade;

        public SlackNotificationEventHandler(ISlackFacade slackFacade)
        {
            _slackFacade = slackFacade;
        }

        public void Handle(SendNotificationDomainEvent domainEvent)
        {
            _slackFacade.SendNotification(domainEvent.Recipient, domainEvent.Message);
        }
    }
}
