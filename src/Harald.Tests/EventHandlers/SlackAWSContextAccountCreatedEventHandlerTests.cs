
using System.Threading.Tasks;
using Harald.Tests.Builders;
using Harald.Tests.TestDoubles;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.EventHandlers;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Xunit;

namespace Harald.Tests.EventHandlers
{
    public class SlackAWSContextAccountCreatedEventHandlerTests
    {

        [Fact]
        public void can_handle_domain_event()
        {
            var sut = new SlackAWSContextAccountCreatedEventHandler(new StubSlackFacade(false));
            var @event = DomainEventBuilder.BuildAWSContextAccountCreatedEvent();

            sut.HandleAsync(@event);

        }
    }

    public class SlackAWSContextAccountCreatedEventHandler : IEventHandler<AWSContextAccountCreatedDomainEvent>
    {
        private readonly ISlackFacade _slackFacade;

        public SlackAWSContextAccountCreatedEventHandler(ISlackFacade slackFacade)
        {
            _slackFacade = slackFacade;
        }

        public Task HandleAsync(AWSContextAccountCreatedDomainEvent domainEvent)
        {
<<<<<<< Updated upstream
=======
            _slackFacade.SendNotificationToChannel()
>>>>>>> Stashed changes
            
            return Task.CompletedTask;
        }
    }
}