using System;
using System.Collections.Generic;
using System.Linq;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.EventHandlers;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public class DomainEventRegistry
    {
        private readonly List<DomainEventRegistration> _registrations = new List<DomainEventRegistration>();
        
        public DomainEventRegistry Register<TEvent>(string eventTypeName, string topicName)
        {
            _registrations.Add(new DomainEventRegistration
            {
                EventType = eventTypeName,
                EventInstanceType = typeof(TEvent),
                Topic = topicName
            });

            return this;
        }

        public string GetTopicFor(string eventType)
        {
            var registration = _registrations.SingleOrDefault(x => x.EventType == eventType);

            if (registration != null)
            {
                return registration.Topic;
            }

            return null;
        }

        public IEnumerable<string> GetAllTopics()
        {
            var topics = _registrations.Select(x => x.Topic).Distinct();           

            return topics;
        }

        public Type GetInstanceTypeFor(string eventType)
        {
            var registration = _registrations.SingleOrDefault(x => x.EventType == eventType);

            if (registration == null)
            {
                throw new MessagingException($"Error! Could not determine \"event instance type\" due to no registration was found for type {eventType}!");
            }

            return registration.EventInstanceType;
        }

        public class DomainEventRegistration
        {
            public string EventType { get; set; }
            public Type EventInstanceType { get; set; }
            public string Topic { get; set; }
        }
    }

}