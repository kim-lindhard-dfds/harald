using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILogger<EventDispatcher> _logger;
        private readonly DomainEventRegistry _eventRegistry;
        private readonly EventHandlerFactory _eventHandlerFactory;
        private readonly ExternalEventMetaDataStore _externalEventMetaDataStore;
        public EventDispatcher(
            ILogger<EventDispatcher> logger,
            DomainEventRegistry eventRegistry,
            EventHandlerFactory eventHandlerFactory, 
            ExternalEventMetaDataStore externalEventMetaDataStore
        )
        {
            _logger = logger;
            _eventRegistry = eventRegistry;
            _eventHandlerFactory = eventHandlerFactory;
            _externalEventMetaDataStore = externalEventMetaDataStore;
        }

        public async Task Send(string generalDomainEventJson, IServiceScope serviceScope)
        {
            try
            {
                var generalDomainEventObj = JsonConvert.DeserializeObject<ExternalEvent>(generalDomainEventJson);
                await SendAsync(generalDomainEventObj, serviceScope);
            }
            catch (JsonReaderException ex)
            {
                throw new EventMessageIncomprehensible($"Received a message that could not be deserialized from expected JSON structure. Original exception: {ex}");
            }
        }

        public async Task SendAsync(ExternalEvent externalEvent, IServiceScope serviceScope)
        {
            if (externalEvent == null)
            {
                throw new EventMessageIncomprehensible("Received a blank message");
            }

            _externalEventMetaDataStore.Store(
                externalEvent.Version,
                externalEvent.EventName,
                externalEvent.XCorrelationId,
                externalEvent.XSender
            );
            
            using (LogContext.PushProperty("CorrelationId", externalEvent.XCorrelationId))
            {
                var eventType = _eventRegistry.GetInstanceTypeFor(externalEvent.EventName);
                dynamic domainEvent = Activator.CreateInstance(eventType, externalEvent);
                dynamic handlersList = _eventHandlerFactory.GetEventHandlersFor(domainEvent, serviceScope);
            
                foreach (var handler in handlersList)
                {
                    await handler.HandleAsync(domainEvent);
                }
            }
            
        }
    }
}