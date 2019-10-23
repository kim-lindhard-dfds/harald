using System;
using System.Threading.Tasks;
using Dafda.Messaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Harald.WebApi.Domain.Events
{
    public class CapabilityCreatedDomainEvent
    {
        public Guid CapabilityId { get; set; }
        public string CapabilityName { get; set; }

        public CapabilityCreatedDomainEvent(Guid capabilityId, string capabilityName)
        {
            CapabilityId = capabilityId;
            CapabilityName = capabilityName;
        }
        
        public CapabilityCreatedDomainEvent() {}
    }
}