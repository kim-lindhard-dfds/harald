using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class MemberJoinedCapabilityDomainEvent : IDomainEvent<MemberJoinedCapabilityData>
    {
        public MemberJoinedCapabilityData Payload { get; }

        public MemberJoinedCapabilityDomainEvent(ExternalEvent domainEvent)
        {
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