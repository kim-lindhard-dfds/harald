using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class CapabilityCreatedDomainEvent : IDomainEvent<CapabilityCreatedData>
    {


        public CapabilityCreatedDomainEvent(ExternalEvent domainEvent)
        {
            Payload = (domainEvent.Payload as JObject)?.ToObject<CapabilityCreatedData>();
        }

        public CapabilityCreatedData Payload { get; }
    }
    public class CapabilityCreatedData
    {
        public Guid CapabilityId { get; private set; }
        public string CapabilityName { get; private set; }

        public CapabilityCreatedData(Guid capabilityId, string capabilityName)
        {
            CapabilityId = capabilityId;
            CapabilityName = capabilityName;
        }
    }
}