using System;
using System.Collections.Generic;
using System.Linq;
using Harald.WebApi.Domain.Events;
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

        public IEnumerable<IEventHandler<TEvent>> GetEventHandlersFor<TEvent>(TEvent domainEvent, IServiceScope serviceScope)
        {
            var eventHandlers = Create<TEvent>(serviceScope);

            if (eventHandlers == null || eventHandlers.Count() == 0)
            {
                throw new MessagingException($"Error! Could not determine \"event handlers\" for type {domainEvent.GetType().FullName}!");
            }

            return eventHandlers;
        }
    }

}