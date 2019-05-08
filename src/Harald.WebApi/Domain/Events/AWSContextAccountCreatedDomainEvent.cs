using System;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class AWSContextAccountCreatedDomainEvent : IDomainEvent<AWSContextAccountCreatedData>
    {
        public Guid MessageId { get; private set; }

        public string Type { get; private set; }

        public AWSContextAccountCreatedData Data { get; private set; }

        public AWSContextAccountCreatedDomainEvent(GeneralDomainEvent domainEvent)
        {
            MessageId = domainEvent.MessageId;
            Type = domainEvent.Type;
            Data = (domainEvent.Data as JObject)?.ToObject<AWSContextAccountCreatedData>();
        }
    }

    public class AWSContextAccountCreatedData
    {
        public Guid ContextId { get; private set; }
        public string AccountId { get; private set; }
        public string RoleArn { get; private set; }
        public string RoleEmail { get; private set; }
        
        public AWSContextAccountCreatedData(Guid contextId, string accountId, string roleArn, string roleEmail)
        {
            ContextId = contextId;
            AccountId = accountId;
            RoleArn = roleArn;
            RoleEmail = roleEmail;

        }
    }
}