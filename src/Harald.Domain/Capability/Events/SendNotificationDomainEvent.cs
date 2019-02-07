using System;

namespace Harald.Domain.Capability.Events
{
    public class SendNotificationDomainEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; private set; }
        public int Version { get; private set; }
        public string Recipient { get; private set; }
        public string Message { get; private set; }

        public SendNotificationDomainEvent(
            Guid aggregateRootId,
            int version,
            string recipient,
            string message)
        {
            AggregateRootId = aggregateRootId;
            Version = version;
            Recipient = recipient;
            Message = message;
        }
    }
}