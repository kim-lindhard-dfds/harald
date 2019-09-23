using System.Threading.Tasks;
using Harald.Infrastructure.Slack.Dto;
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

        public async Task<UserGroupDto> EnsureUserGroupExists(string capabilityName)
        {
            EnsureUserGroupExistsHasBeenCalled = true;
            return await _successor.EnsureUserGroupExists(capabilityName);
        }
    }
}