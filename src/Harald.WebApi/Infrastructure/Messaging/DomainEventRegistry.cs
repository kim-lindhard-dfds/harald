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
        
        public DomainEventRegistry Register<TEvent>(string eventName, string topicName)
        {
            _registrations.Add(new DomainEventRegistration
            {
                EventName = eventName,
                EventInstanceType = typeof(TEvent),
                Topic = topicName
            });

            return this;
        }

        public string GetTopicFor(string eventType)
        {
            var registration = _registrations.SingleOrDefault(x => x.EventName == eventType);

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

        public Type GetInstanceTypeFor(string eventName)
        {
            var registration = _registrations.SingleOrDefault(x => x.EventName == eventName);

            if (registration == null)
            {
                throw new EventTypeNotFoundException($"Error! Could not determine \"event instance type\" due to no registration was found for type {eventName}!");
            }

            return registration.EventInstanceType;
        }

        public class DomainEventRegistration
        {
            public string EventName { get; set; }
            public Type EventInstanceType { get; set; }
            public string Topic { get; set; }
        }
    }

}