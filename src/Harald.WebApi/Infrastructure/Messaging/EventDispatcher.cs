using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.EventHandlers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILogger<EventDispatcher> _logger;
        private readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();
        private Dictionary<string, Type> _eventTypeMap = new Dictionary<string, Type>();
        
        public EventDispatcher(
            ILogger<EventDispatcher> logger,
            IEventHandler<CapabilityCreatedDomainEvent> capabilityCreatedEventhandler)
        {
            _logger = logger;
            Register(capabilityCreatedEventhandler);
        }

        private void Register<T>(IEventHandler<T> handler)
        {
            if (!_handlers.ContainsKey(handler.EventTypeImplementation))
            {
                _handlers.Add(handler.EventTypeImplementation, new List<object>());
            }
            
            List<object> handlersList = _handlers[handler.EventTypeImplementation];
            handlersList.Add(handler);

            if (!_eventTypeMap.ContainsKey(handler.EventType))
            {
                _eventTypeMap.Add(handler.EventType, handler.EventTypeImplementation);
            }
        }

        public async Task Send(string generalDomainEventJson)
        {
            var generalDomainEventObj = JsonConvert.DeserializeObject<GeneralDomainEvent>(generalDomainEventJson);
            await SendAsync(generalDomainEventObj);
        }

        public async Task SendAsync(GeneralDomainEvent generalDomainEvent)
        {
            var eventType = _eventTypeMap[generalDomainEvent.Type];
            dynamic domainEvent = Activator.CreateInstance(eventType, generalDomainEvent);

            dynamic handlersList = Convert.ChangeType(_handlers[eventType], typeof(List<object>));

            foreach (var handler in handlersList)
            {
                await handler.HandleAsync(domainEvent);
            }
        }
    }
}