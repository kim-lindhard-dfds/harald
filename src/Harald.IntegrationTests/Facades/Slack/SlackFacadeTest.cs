using System;
using System.Net.Http;
using Xunit;
using System.Net;
using System.Threading.Tasks;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Harald.WebApi.Infrastructure.Serialization;
using Xunit.Priority;

namespace Harald.IntegrationTests.Facades.Slack
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class SlackFacadeTest : IClassFixture<SlackFacadeTestFixture>
    {
        private SlackFacadeTestFixture _data;
        public SlackFacadeTest(SlackFacadeTestFixture data)
        {
            _data = data;
        }

        [Fact, Priority(0)]
        public async Task CreateChannel_Given_valid_input_Should_create_channel()
        {
            // Arrange
            var httpClient = GetHttpClient();
            var sut = new SlackFacade(httpClient, new JsonSerializer(), new SlackHelper());
            const string channelName = "ded-team-one";

            // Act
            var createChannelResponse = await sut.CreateChannel(channelName);

            // Assert
            Assert.True(createChannelResponse.Ok);
            Assert.Equal(channelName, createChannelResponse.Channel.Name);
            Assert.NotEmpty(createChannelResponse.Channel.Id);
            
            // For eventual cleanup
            _data.UserChannelId = createChannelResponse.Channel.Id;
        }

        [Fact]
        public async Task InviteToChannel_Given_valid_input_Should_invite_to_channel()
        {
            // Arrange
            var httpClient = GetHttpClient();
            var sut = new SlackFacade(httpClient, new JsonSerializer(), new SlackHelper());
            var conversations = await sut.GetConversations();
            var channelId = conversations.GetChannel("ded-team-one").Id;
            var userEmail = GetUserEmail();

            // Act
            await sut.InviteToChannel(userEmail, channelId);

            // Assert
        }

        [Fact]
        public async Task CreateUserGroup_And_Add_User_Given_valid_input_Should_create_group_with_user()
        {
            // Arrange
            var httpClient = GetHttpClient();
            var sut = new SlackFacade(httpClient, new JsonSerializer(), new SlackHelper());
            const string groupName = "Harald Integration Test Group";
            const string handle = "harald-int-a";
            const string description = "Group created through integration test.";
            var userEmail = GetUserEmail();

            // Act
            var createUserGroupResponse = await sut.CreateUserGroup(name: groupName, handle: handle, description: description);
            if (!createUserGroupResponse.Ok)
            {
                throw new SlackFacadeException($"API error: {createUserGroupResponse.Error}");
            }
            
            await sut.AddUserGroupUser(userGroupId: createUserGroupResponse.UserGroup.Id, email: userEmail);

            // Assert
            Assert.True(createUserGroupResponse.Ok);
            Assert.Equal(groupName, createUserGroupResponse.UserGroup.Name);
            // For eventual cleanup
            _data.UserGroupId = createUserGroupResponse.UserGroup.Id;
        }

        [Fact]
        public async Task SendNotificationToChannel_Given_valid_input_Should_send_notfication_to_channel()
        {
            // Arrange
            var httpClient = GetHttpClient();
            var sut = new SlackFacade(httpClient, new JsonSerializer(), new SlackHelper());
            const string channel = "ded-team-one";
            const string message = "Integration test message.";

            // Act
            var sendNotificationToChannelResponse = await sut.SendNotificationToChannel(channel: channel, message: message);

            // Assert
            Assert.True(sendNotificationToChannelResponse.Ok);
            Assert.NotEmpty(sendNotificationToChannelResponse.TimeStamp);
        }

        [Fact]
        public async Task PinMessageToChannel_Given_valid_input_Should_send_notfication_to_channel()
        {
            // Arrange
            var httpClient = GetHttpClient();
            var sut = new SlackFacade(httpClient, new JsonSerializer(), new SlackHelper());
            const string channel = "ded-team-one";
            const string message = "Integration test message.";

            var conversations = await sut.GetConversations();
            var slackChannelObj = conversations.GetChannel(channel);

            // Act
            var sendNotificationToChannelResponse = await sut.SendNotificationToChannel(channel: channel, message: message);
            var pinMessageToChannelResponse = await sut.PinMessageToChannel(channel: slackChannelObj.Id, messageTimeStamp: sendNotificationToChannelResponse.TimeStamp);

            // Assert
            Assert.True(sendNotificationToChannelResponse.Ok);
            Assert.NotEmpty(sendNotificationToChannelResponse.TimeStamp);
            Assert.True(pinMessageToChannelResponse.Ok);
        }
        

        static public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://slack.com");
            var authToken = Environment.GetEnvironmentVariable("SLACK_API_AUTH_TOKEN");
            
            httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {authToken}");

            return httpClient;
        }

        private string GetUserEmail()
        {
            return Environment.GetEnvironmentVariable("SLACK_TESTING_USER_EMAIL");
        }
    }

    public class SlackFacadeTestFixture : IDisposable
    {
        public string UserChannelId { get; set; }
        public string UserGroupId { get; set; }
        public Config Configuration { get; set; }
        
        public SlackFacadeTestFixture()
        {
            Configuration = new Config();
        }

        public void Dispose()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[12];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var nameAndHandle = new String(stringChars);
            
            var httpClient = SlackFacadeTest.GetHttpClient();
            var sut = new SlackFacade(httpClient, new JsonSerializer(), new SlackHelper());
            if (UserGroupId != null)
            {
                sut.RenameUserGroup(UserGroupId, nameAndHandle, nameAndHandle + "Handle").Wait();
            }

            if (UserChannelId != null)
            {
                sut.RenameChannel(UserChannelId, nameAndHandle).Wait();
            }
        }
    }

    public class Config
    {
        public string TestingUserEmail { get; set; }
        public string TestingSlackApiAuthToken { get; set; }

        public Config()
        {
            TestingUserEmail = GetString("SLACK_TESTING_USER_EMAIL", "hellopelle@dfds.com");
            TestingSlackApiAuthToken = GetString("SLACK_API_AUTH_TOKEN", "replaceme");
        }
        
        internal string GetString(string envVarName, string defaultValue)
        {
            return Environment.GetEnvironmentVariable(envVarName) != null
                ? Environment.GetEnvironmentVariable(envVarName)
                : defaultValue;
        }
    }
}
