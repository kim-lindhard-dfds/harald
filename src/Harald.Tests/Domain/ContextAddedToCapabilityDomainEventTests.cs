using System;
using Harald.WebApi.Domain.Events;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Harald.Tests.Domain
{
    public class ContextAddedToCapabilityDomainEventTests
    {
        [Fact]
        public void can_create_valid_ContextAddedToCapabilityDomainEvent_when_given_GeneralDomainEvent()
        {
            // Arrange
            var CapabilityId = Guid.NewGuid();
            var ContextId = Guid.NewGuid();
            var ContextName = "foo";
            
            dynamic data = new JObject();
            data.capabilityId = CapabilityId;
            data.contextId = ContextId;
            data.contextName = ContextName;
            
            
            var messageId = Guid.NewGuid();
            var generalDomainEvent = new GeneralDomainEvent(
                messageId: messageId,
                type: "context_added_to_capability",
                data: data
            );
            
            
            // Act 
            var contextAddedToCapabilityDomainEvent = new ContextAddedToCapabilityDomainEvent(generalDomainEvent);
            
            
            // Assert
            Assert.Equal(CapabilityId, contextAddedToCapabilityDomainEvent.Data.CapabilityId);
            Assert.Equal(ContextId, contextAddedToCapabilityDomainEvent.Data.ContextId);
            Assert.Equal(ContextName, contextAddedToCapabilityDomainEvent.Data.ContextName);
        }
    }
}