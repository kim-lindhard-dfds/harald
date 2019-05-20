using System;
using Harald.Tests.Domain.Events;
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
                Guid.NewGuid(), "context_added_to_capability", new ContextAddedToCapabilityData(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "foo"
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