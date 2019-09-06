using System.Collections.Generic;
using System.Linq;
using Harald.WebApi.Application.EventHandlers;
using Harald.WebApi.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public class EventHandlerFactory
    {
        public IEnumerable<IEventHandler<TEvent>> Create<TEvent>(IServiceScope serviceScope)
        {
            var eventHandlers = serviceScope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
            return eventHandlers;
        }

        public IEnumerable<IEventHandler<TEvent>> GetEventHandlersFor<TEvent>(TEvent domainEvent,
            IServiceScope serviceScope)
        {
            var eventHandlers = Create<TEvent>(serviceScope);

            if (eventHandlers == null || eventHandlers.Count() == 0)
            {
                throw new EventHandlerNotFoundException(
                    $"Error! Could not determine \"event handlers\" due to no registration was found for type {domainEvent.GetType().FullName}!");
            }

            return eventHandlers;
        }
    }
}