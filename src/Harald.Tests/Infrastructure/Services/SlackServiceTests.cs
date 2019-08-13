using System;
using System.Threading.Tasks;
using Harald.Tests.TestDoubles;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Harald.WebApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Harald.Tests.Infrastructure.Services
{
    public class SlackServiceTests
    {
    
        [Fact]
        public void TestWeCallSlackFacadeAndCreateUserGroupOnlyIfNotExisting()
        {
            var slackFacadeSpy = new SlackFacadeSpy();
            var logger = new LoggerFactory().CreateLogger<SlackService>();
            var slackHelper = new SlackHelper();
            var sut = new SlackService(slackFacadeSpy, slackHelper, logger);

            var capabilityName = "foo";

           sut.EnsureUserGroupExists(capabilityName);
            
            Assert.True(slackFacadeSpy.CreateUserGroupWasCalled);
            Assert.Equal(capabilityName, slackFacadeSpy.CreateUserGroupHandle);

        }
        
        
        [Fact]
        public async Task TestWeCallSlackFacadeAndNotCreateUserGroup()
        {
            var slackFacadeSpy = new SlackFacadeSpy();
            var logger = new LoggerFactory().CreateLogger<SlackService>();
            var slackHelper = new SlackHelper();
            var sut = new SlackService(slackFacadeSpy, slackHelper, logger);
            
            slackFacadeSpy.UserGroups.Add(new UserGroup
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