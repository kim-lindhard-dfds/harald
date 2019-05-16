using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class CapabilityCreatedDomainEvent : IDomainEvent<CapabilityCreatedData>
    {
      

        public CapabilityCreatedDomainEvent(GeneralDomainEvent domainEvent)
        {
            Version = domainEvent.Version;
            EventName = domainEvent.EventName;
            XCorrelationId = domainEvent.XCorrelationId;
            XSender = domainEvent.XSender;
            Payload = (domainEvent.Payload as JObject)?.ToObject<CapabilityCreatedData>();
        }

        public string Version { get; }
        public string EventName { get; }
        public Guid XCorrelationId { get; }
        public string XSender { get; }
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