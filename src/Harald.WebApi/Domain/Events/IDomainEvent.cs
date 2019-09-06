using System;

namespace Harald.WebApi.Domain.Events
{
    public interface IDomainEvent<T>
    {
        T Payload { get; }
    }
}