using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class K8sNamespaceCreatedAndAwsArnConnectedDomainEvent : IDomainEvent<K8sNamespaceCreatedAndAwsArnConnectedData>
    {
        public K8sNamespaceCreatedAndAwsArnConnectedDomainEvent(IntegrationEvent integrationEvent)
        {
            Payload = (integrationEvent.Payload as JObject)?.ToObject<K8sNamespaceCreatedAndAwsArnConnectedData>();
        }

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