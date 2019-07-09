using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class ContextAddedToCapabilityDomainEvent : IDomainEvent<ContextAddedToCapabilityData>
    {
        public string Version { get; }
        public string EventName { get; }
        public string XCorrelationId { get; }
        public string XSender { get; }
        public ContextAddedToCapabilityData Payload { get; }

        public ContextAddedToCapabilityDomainEvent(GeneralDomainEvent domainEvent)
        {
            Version = domainEvent.Version;
            EventName = domainEvent.EventName;
            XCorrelationId = domainEvent.XCorrelationId;
            XSender = domainEvent.XSender;
            Payload = (domainEvent.Payload as JObject)?.ToObject<ContextAddedToCapabilityData>();
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