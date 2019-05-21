using System;

namespace Harald.WebApi.Domain.Events
{
    public interface IDomainEvent<T>
    {
        string Version { get; }
        string EventName { get; }
        Guid XCorrelationId { get; }
        string XSender { get; }
        T Payload { get; }
    }
}