using System;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.EventHandlers;
using Xunit;

namespace Harald.Tests.EventHandlers
{
    public class SlackContextAddedToCapabilityDomainEventHandlerTests
    {
        [Fact]
        public void CreateMessage_can_handle_null_capability()
        {
            // Arrange
            var contextAddedToCapabilityDomainEvent = new ContextAddedToCapabilityDomainEvent(new GeneralDomainEvent(
                "1",
                "context_added_to_capability",
                Guid.NewGuid(),
                "myFqdn", new ContextAddedToCapabilityData(
                    Guid.NewGuid(),
                    "capabilityName",
                    "foo",
                    Guid.NewGuid(),
                    "contextName"
                )));
            
            Capability capability = null;


            // Act / Assert
            SlackContextAddedToCapabilityDomainEventHandler.CreateMessage(
                contextAddedToCapabilityDomainEvent,
                capability
            );
        }
    }
}