using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;

namespace Harald.WebApi.EventHandlers
{
    public class SlackContextAddedToCapabilityDomainEvent : IEventHandler<ContextAddedToCapabilityDomainEvent>
    {
        public Task HandleAsync(ContextAddedToCapabilityDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}