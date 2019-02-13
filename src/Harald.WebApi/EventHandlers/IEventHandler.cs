using System;
using System.Threading.Tasks;

namespace Harald.WebApi.EventHandlers
{
    public interface IEventHandler<in T>
    {
        string EventType { get; }
        Type EventTypeImplementation { get; }
        Task HandleAsync(T domainEvent);
    }
}