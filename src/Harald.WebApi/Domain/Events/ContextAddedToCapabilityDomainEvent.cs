using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class ContextAddedToCapabilityDomainEvent : IDomainEvent<ContextAddedToCapabilityData>
    {
        public ContextAddedToCapabilityData Payload { get; }

        public ContextAddedToCapabilityDomainEvent(IntegrationEvent integrationEvent)
        {
            Payload = (integrationEvent.Payload as JObject)?.ToObject<ContextAddedToCapabilityData>();
        }

        public ContextAddedToCapabilityDomainEvent(ContextAddedToCapabilityData payload)
        {
            Payload = payload;
        }

        public static ContextAddedToCapabilityDomainEvent Create(
            Guid capabilityId,
            string capabilityName,
            string capabilityRootId,
            Guid contextId,
            string contextName
        )
        {
            var payload = new ContextAddedToCapabilityData(
                capabilityId,
                capabilityName,
                capabilityRootId,
                contextId,
                capabilityName
            );

            var contextAddedToCapabilityDomainEvent = new ContextAddedToCapabilityDomainEvent(payload);

            
            return contextAddedToCapabilityDomainEvent;
        }
    }


    public class ContextAddedToCapabilityData
    {
        public Guid CapabilityId { get; }
        public string CapabilityName { get; }
        public string CapabilityRootId { get; }
        public Guid ContextId { get; }
        public string ContextName { get; }


        public ContextAddedToCapabilityData(
            Guid capabilityId,
            string capabilityName,
            string capabilityRootId,
            Guid contextId,
            string contextName
        )
        {
            CapabilityId = capabilityId;
            CapabilityName = capabilityName;
            CapabilityRootId = capabilityRootId;
            ContextId = contextId;
            ContextName = contextName;
        }
    }
}