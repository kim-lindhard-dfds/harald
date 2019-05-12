using Harald.Tests.Builders;
using Harald.Tests.TestDoubles;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.EventHandlers;
using Xunit;

namespace Harald.Tests.EventHandlers
{
    public class SlackAWSContextAccountCreatedEventHandlerTests
    {

        [Fact]
        public void can_handle_domain_event()
        {
            var sut = new SlackAWSContextAccountCreatedEventHandler(new StubSlackFacade(false));
            var eventData = DomainEventBuilder.BuildAWSContextAccountCreatedEventData();
            var @event = new AWSContextAccountCreatedDomainEvent(eventData);

            sut.HandleAsync(@event);

        }
    }
}