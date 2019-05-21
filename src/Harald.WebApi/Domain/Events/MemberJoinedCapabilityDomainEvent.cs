using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class MemberJoinedCapabilityDomainEvent : IDomainEvent<MemberJoinedCapabilityData>
    {
        public string Version { get; }
        public string EventName { get; }
        public Guid XCorrelationId { get; }
        public string XSender { get; }
        public MemberJoinedCapabilityData Payload { get; }

        public MemberJoinedCapabilityDomainEvent(GeneralDomainEvent domainEvent)
        {
            Version = domainEvent.Version;
            EventName = domainEvent.EventName;
            XCorrelationId = domainEvent.XCorrelationId;
            XSender = domainEvent.XSender;
            Payload = (domainEvent.Payload as JObject)?.ToObject<MemberJoinedCapabilityData>();
        }

    
    }

    public class MemberJoinedCapabilityData
    {
        public Guid CapabilityId { get; }
        public string MemberEmail { get; }

        public MemberJoinedCapabilityData(Guid capabilityId, string memberEmail)
        {
            CapabilityId = capabilityId;
            MemberEmail = memberEmail;
        }
    }
}