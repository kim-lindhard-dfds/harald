using System;

namespace Harald.Domain.Capability.Events
{
    public class CapabilityCreatedDomainEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; private set; }
        public int Version { get; private set; }
        public string CapabilityId { get; private set; }
        public string CapabilityName { get; private set; }

        public CapabilityCreatedDomainEvent(
            Guid aggregateRootId,
            int version,
            string capabilityId,
            string capabilityName)
        {
            AggregateRootId = aggregateRootId;
            Version = version;
            CapabilityId = capabilityId;
            CapabilityName = capabilityName;
        }
    }
}