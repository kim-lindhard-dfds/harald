using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.Tests.TestDoubles
{
    public class StubCapabilityRepository : ICapabilityRepository
    {
        private readonly List<Guid> _capabilityIds;
        public StubCapabilityRepository(List<Guid> capabilityIds)
        {
            _capabilityIds = capabilityIds;
        }

        public Task Add(Capability capability)
        {
            _capabilityIds.Add(capability.Id);
            return Task.FromResult<object>(null);
        }

        public Task Update(Capability capability)
        {
            return Task.CompletedTask;
        }

        public Task<Capability> Get(Guid id)
        {
            var capabilityId = _capabilityIds.FirstOrDefault(capId => capId == id);

            if (capabilityId == default(Guid))
            {
                return Task.FromResult<Capability>(null);
            }

            return Task.FromResult(new Capability(
                id: id,
                name: "FooCapability",
                slackChannelId: "FooChannelId",
                slackUserGroupId: "FooUserGroupId"
            ));
        }
    }
}