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
                var generalDomainEventObj = JsonConvert.DeserializeObject<IntegrationEvent>(generalDomainEventJson);
                await SendAsync(generalDomainEventObj, serviceScope);
            }
            catch (JsonReaderException ex)
            {
                throw new EventMessageIncomprehensible($"Received a message that could not be deserialized from expected JSON structure. Original exception: {ex}");
            }
        }

        public async Task SendAsync(IntegrationEvent integrationEvent, IServiceScope serviceScope)
        {
            if (integrationEvent == null)
            {
                throw new EventMessageIncomprehensible("Received a blank message");
            }

            _externalEventMetaDataStore.Store(
                integrationEvent.Version,
                integrationEvent.EventName,
                integrationEvent.XCorrelationId,
                integrationEvent.XSender
            );
            
            using (LogContext.PushProperty("CorrelationId", integrationEvent.XCorrelationId))
            {
                var eventType = _eventRegistry.GetInstanceTypeFor(integrationEvent.EventName);
                dynamic domainEvent = Activator.CreateInstance(eventType, integrationEvent);
                dynamic handlersList = _eventHandlerFactory.GetEventHandlersFor(domainEvent, serviceScope);
            
                foreach (var handler in handlersList)
                {
                    await handler.HandleAsync(domainEvent);
                }
            }
            
        }
    }
}