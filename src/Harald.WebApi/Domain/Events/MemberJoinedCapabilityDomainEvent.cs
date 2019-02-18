using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class MemberJoinedCapabilityDomainEvent : IDomainEvent<MemberJoinedCapabilityData>
    {
        public Guid MessageId { get; private set; }

        public string Type { get; private set; }

        public MemberJoinedCapabilityData Data { get; private set; }

        public MemberJoinedCapabilityDomainEvent(GeneralDomainEvent domainEvent)
        {
            MessageId = domainEvent.MessageId;
            Type = domainEvent.Type;
            Data = (domainEvent.Data as JObject)?.ToObject<MemberJoinedCapabilityData>();
        }
    }

    public class MemberJoinedCapabilityData
    {
        public Guid CapabilityId { get; private set; }
        public string MemberEmail { get; private set; }

        public MemberJoinedCapabilityData(Guid capabilityId, string memberEmail)
        {
            CapabilityId = capabilityId;
            MemberEmail = memberEmail;
        }
    }
}