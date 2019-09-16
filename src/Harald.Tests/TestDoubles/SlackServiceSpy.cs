using System.Threading.Tasks;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Harald.WebApi.Infrastructure.Services;

namespace Harald.Tests.TestDoubles
{
    public class SlackServiceSpy : ISlackService
    {
        private readonly ISlackService _successor;
        public SlackServiceSpy(ISlackService successor)
        {
            _successor = successor;
        }
        public bool EnsureUserGroupExistsHasBeenCalled { get; private set; }

        public async Task<UserGroup> EnsureUserGroupExists(string capabilityName)
        {
            EnsureUserGroupExistsHasBeenCalled = true;
            return await _successor.EnsureUserGroupExists(capabilityName);
        }
    }
}