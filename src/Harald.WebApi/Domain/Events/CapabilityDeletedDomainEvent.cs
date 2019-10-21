using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class CapabilityDeletedDomainEvent : IDomainEvent<CapabilityDeletedData>
    {
        public CapabilityDeletedDomainEvent(ExternalEvent domainEvent)
        {
            Payload = (domainEvent.Payload as JObject)?.ToObject<CapabilityDeletedData>();
        }

        public CapabilityDeletedDomainEvent(CapabilityDeletedData payload)
        {
            Payload = payload;
        }

        public static CapabilityDeletedDomainEvent Create(
            Guid capabilityId, 
            string capabilityName
        )
        {
            var payload = new CapabilityDeletedData(capabilityId, capabilityName);
            return new CapabilityDeletedDomainEvent(payload);
        }

        public CapabilityDeletedData Payload { get; }
    }

    public class CapabilityDeletedData
    {
        public Guid CapabilityId { get; private set; }
        public string CapabilityName { get; private set; }

        public CapabilityDeletedData(Guid capabilityId, string capabilityName)
        {
            CapabilityId = capabilityId;
            CapabilityName = capabilityName;
        }
    }
}