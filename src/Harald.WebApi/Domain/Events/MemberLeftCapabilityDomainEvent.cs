using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class MemberLeftCapabilityDomainEvent : IDomainEvent<MemberLeftCapabilityData>
    {
        public Guid MessageId { get; private set; }

        public string Type { get; private set; }

        public MemberLeftCapabilityData Data { get; private set; }

        public MemberLeftCapabilityDomainEvent(GeneralDomainEvent domainEvent)
        {
            MessageId = domainEvent.MessageId;
            Type = domainEvent.Type;
            Data = (domainEvent.Data as JObject)?.ToObject<MemberLeftCapabilityData>();
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