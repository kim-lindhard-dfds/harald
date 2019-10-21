using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;

namespace Harald.WebApi.Application.EventHandlers
{
    public class CapabilityDeletedEventNotifyDedHandler : IEventHandler<CapabilityDeletedDomainEvent>
    {
        public Task HandleAsync(CapabilityDeletedDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}