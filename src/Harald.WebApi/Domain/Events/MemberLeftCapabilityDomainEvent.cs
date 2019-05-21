using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class MemberLeftCapabilityDomainEvent : IDomainEvent<MemberLeftCapabilityData>
    {
        public string Version { get; }
        public string EventName { get; }
        public Guid XCorrelationId { get; }
        public string XSender { get; }
        public MemberLeftCapabilityData Payload { get; }
        public MemberLeftCapabilityDomainEvent(GeneralDomainEvent domainEvent)
        {
            Version = domainEvent.Version;
            EventName = domainEvent.EventName;
            XCorrelationId = domainEvent.XCorrelationId;
            XSender = domainEvent.XSender;
            Payload = (domainEvent.Payload as JObject)?.ToObject<MemberLeftCapabilityData>();
        }
    }

    public class MemberLeftCapabilityData
    {
        public Guid CapabilityId { get; private set; }
        public string MemberEmail { get; private set; }

        public MemberLeftCapabilityData(Guid capabilityId, string memberEmail)
        {
            CapabilityId = capabilityId;
            MemberEmail = memberEmail;
        }
    }
}