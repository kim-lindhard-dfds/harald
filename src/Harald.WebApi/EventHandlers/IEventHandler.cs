using System;
using System.Threading.Tasks;

namespace Harald.WebApi.EventHandlers
{
    public interface IEventHandler<in T>
    {
        Task HandleAsync(T domainEvent);
    }
}