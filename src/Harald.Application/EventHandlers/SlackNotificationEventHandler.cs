using System.Threading.Tasks;
using Harald.Application.Facades.Slack;
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

        public async Task HandleAsync(SendNotificationDomainEvent domainEvent)
        {
            await _slackFacade.SendNotification(domainEvent.Recipient, domainEvent.Message);
        }
    }
}
