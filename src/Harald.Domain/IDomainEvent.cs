using System;

namespace Harald.Domain
{
    public interface IDomainEvent
    {
        Guid AggregateRootId { get; }
        int Version { get; }
    }
}
