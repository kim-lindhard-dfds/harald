using System;
using Harald.WebApi.Application.EventHandlers;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Harald.Tests.Application.EventHandlers
{
    public class SlackContextAddedToCapabilityDomainEventHandlerTests
    {
        [Fact]
        public void CreateMessage_can_handle_null_capability()
        {
            // Arrange

            dynamic payload = new JObject();
            payload.CapabilityId = Guid.NewGuid();
            payload.CapabilityName = "CapabilityNameHere";
            payload.CapabilityRootId = "blah";
            payload.ContextId = Guid.NewGuid().ToString();
            payload.ContextName = "default";


            var contextAddedToCapabilityDomainEvent = new ContextAddedToCapabilityDomainEvent(new GeneralDomainEvent(
                "1","context_added_to_capability", Guid.NewGuid().ToString(),"", payload));



            Capability capability = null;


            // Act / Assert
            SlackContextAddedToCapabilityDomainEventHandler.CreateMessage(
                contextAddedToCapabilityDomainEvent,
                capability
            );
        }
    }
}