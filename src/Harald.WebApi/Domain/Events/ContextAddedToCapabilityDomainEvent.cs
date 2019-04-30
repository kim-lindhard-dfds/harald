using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class ContextAddedToCapabilityDomainEvent : IDomainEvent<ContextAddedToCapabilityData>
    {
        public Guid MessageId { get; private set; }

        public string Type { get; private set; }

        public ContextAddedToCapabilityData Data { get; private set; }

        public ContextAddedToCapabilityDomainEvent(GeneralDomainEvent domainEvent)
        {
            MessageId = domainEvent.MessageId;
            Type = domainEvent.Type;
            Data = (domainEvent.Data as JObject)?.ToObject<ContextAddedToCapabilityData>();
        }
    }

    public class ContextAddedToCapabilityData
    {
        public Guid CapabilityId { get; private set; }
        public Guid ContextId { get; private set; }
        public string ContextName { get; private set; }

        public ContextAddedToCapabilityData(
            Guid capabilityId, 
            Guid contextId, 
            string contextName
        )
        {
            CapabilityId = capabilityId;
            ContextId = contextId;
            ContextName = contextName;
        }
    }
}