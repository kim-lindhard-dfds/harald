using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class K8sNamespaceCreatedAndAwsArnConnectedDomainEvent : IDomainEvent<K8sNamespaceCreatedAndAwsArnConnectedData>
    {
        public K8sNamespaceCreatedAndAwsArnConnectedDomainEvent(GeneralDomainEvent domainEvent)
        {
            Version = domainEvent.Version;
            EventName = domainEvent.EventName;
            XCorrelationId = domainEvent.XCorrelationId;
            XSender = domainEvent.XSender;
            Payload = (domainEvent.Payload as JObject)?.ToObject<K8sNamespaceCreatedAndAwsArnConnectedData>();
        }

        public string Version { get; }
        public string EventName { get; }
        public string XCorrelationId { get; }
        public string XSender { get; }
        public K8sNamespaceCreatedAndAwsArnConnectedData Payload { get; }
    }
    public class K8sNamespaceCreatedAndAwsArnConnectedData
    {
        public Guid CapabilityId { get; private set; }
        public Guid ContextId { get; private set; }
        public string NamespaceName { get; private set; }

        public K8sNamespaceCreatedAndAwsArnConnectedData(Guid capabilityId, Guid contextId, string namespaceName)
        {
            CapabilityId = capabilityId;
            ContextId = contextId;
            NamespaceName = namespaceName;
        }
    }
}