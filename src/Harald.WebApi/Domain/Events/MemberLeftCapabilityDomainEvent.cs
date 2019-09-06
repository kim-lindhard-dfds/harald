using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class MemberLeftCapabilityDomainEvent : IDomainEvent<MemberLeftCapabilityData>
    {
        public MemberLeftCapabilityData Payload { get; }
        public MemberLeftCapabilityDomainEvent(ExternalEvent domainEvent)
        {
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