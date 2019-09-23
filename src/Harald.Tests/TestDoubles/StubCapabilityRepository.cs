using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.Infrastructure.Slack;

namespace Harald.Tests.TestDoubles
{
    public class StubCapabilityRepository : ICapabilityRepository
    {
        private readonly List<Capability> _capabilities = new List<Capability>();

        public StubCapabilityRepository(List<Guid> capabilityIds)
        {
            foreach (var capabilityId in capabilityIds)
            {
                _capabilities.Add(
                    Capability.Create(
                        id: capabilityId,
                        name: "FooCapability",
                        slackChannelId: "FooChannelId",
                        slackUserGroupId: "FooUserGroupId"
                    ));
            }
        }

        public StubCapabilityRepository()
        {
        }
        
        public Task Add(Capability capability)
        {
            _capabilities.Add(capability);
            return Task.CompletedTask;
        }

        public Task Update(Capability capability)
        {
            return Task.CompletedTask;
        }


        public Task<Capability> Get(Guid id)
        {
            var capability = _capabilities.FirstOrDefault(c => c.Id == id);

            return Task.FromResult(capability);
        }

        public Task<IEnumerable<Capability>> GetAll()
        {
            return Task.FromResult(_capabilities.AsEnumerable());
        }
    }
}