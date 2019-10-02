using System;
using System.Threading.Tasks;
using Harald.Tests.TestDoubles;
using Harald.WebApi.Domain;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Xunit;
using Harald.Infrastructure.Slack.Dto;

namespace Harald.Tests.Infrastructure.Services
{
    public class SlackServiceTests
    {
    
        [Fact]
        public async Task TestWeCallSlackFacadeAndCreateUserGroupOnlyIfNotExisting()
        {
            var slackFacadeSpy = new SlackFacadeSpy();
            var logger = new LoggerFactory().CreateLogger<SlackService>();
            var sut = new SlackService(slackFacadeSpy, logger);

            var capabilityName = "foo";

            await sut.EnsureUserGroupExists(capabilityName);
            
            Assert.True(slackFacadeSpy.CreateUserGroupWasCalled);
            Assert.Equal(capabilityName + "-members", slackFacadeSpy.CreateUserGroupHandle);

        }
        
        
        [Fact]
        public async Task TestWeCallSlackFacadeAndNotCreateUserGroup()
        {
            var slackFacadeSpy = new SlackFacadeSpy();
            var logger = new LoggerFactory().CreateLogger<SlackService>();
            var sut = new SlackService(slackFacadeSpy, logger);
            
            slackFacadeSpy.UserGroups.Add(new UserGroupDto
            {
                Handle = "foocapability-members",
                Id = "bar",
                Name = "foo"
            });

            var capabilityName = "foocapability";

            await sut.EnsureUserGroupExists(capabilityName);
            
            Assert.False(slackFacadeSpy.CreateUserGroupWasCalled);
        }
    }
}