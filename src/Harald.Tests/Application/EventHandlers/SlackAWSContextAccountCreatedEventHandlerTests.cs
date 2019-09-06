using System;
using System.Collections.Generic;
using Harald.Tests.Builders;
using Harald.Tests.TestDoubles;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.EventHandlers;
using Xunit;

namespace Harald.Tests.Application.EventHandlers
{
    public class SlackAWSContextAccountCreatedEventHandlerTests
    {

        [Fact]
        public void can_handle_domain_event()
        {
            var slackStub = new SlackFacadeStub(false);
            var capabilityRepositoryStub = new StubCapabilityRepository(new List<Guid>());
            var sut = new SlackAwsContextAccountCreatedEventHandler(slackStub, capabilityRepositoryStub);
            var eventData = DomainEventBuilder.BuildAWSContextAccountCreatedEventData();
            var @event = new AWSContextAccountCreatedDomainEvent(eventData);

            sut.HandleAsync(@event);

            Assert.True(slackStub.SendNotificationToChannelCalled);
        }
    }
}