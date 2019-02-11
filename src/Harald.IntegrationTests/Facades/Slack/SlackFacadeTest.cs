using System;
using System.Net.Http;
using Xunit;
using Harald.Infrastructure.Facades.Slack;
using Harald.Infrastructure.Serialization;
using System.Net;
using System.Threading.Tasks;

namespace Harald.IntegrationTests.Facades.Slack
{
    public class SlackFacadeTest
    {
        [Fact]
        public async Task CreateChannel_Given_valid_input_Should_create_channel()
        {
            // Arrange
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://slack.com");
            var authToken = Environment.GetEnvironmentVariable("SLACK_API_AUTH_TOKEN");

            httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {authToken}");

            var sut = new SlackFacade(httpClient, new JsonSerializer());
            var channelName = "janie-test";
            
            // Act
            var createChannelResponse = await sut.CreateChannel(channelName);
            
            // Assert
             Assert.True(createChannelResponse.Ok);
            Assert.Equal(channelName, createChannelResponse.Channel.Name);
            Assert.NotEmpty(createChannelResponse.Channel.Id);
        }
    }
}
