using Harald.Infrastructure.Slack;
using Harald.Tests.Builders;
using Harald.Tests.TestDoubles;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Harald.Tests.Controllers
{
    public class TestChannelsRoute
    {
        [Fact]
        public async Task Get_channels_returns_expected_status_code()
        {
            // Arrange
            var channelType = "slack";

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                // Act
                var response = await client.GetAsync($"api/v1/channels?channelType={channelType}");
                
                // Assert
                var result = await response.Content.ReadAsStringAsync();
                Assert.True(!string.IsNullOrEmpty(result));
                Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            }
        }
        
        
        [Fact]
        public async Task Get_channels_with_unknown_channelType_returns_400_code_and_message()
        {
            // Arrange
            var channelType = "thisIsNotAChannelType";

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                
                // Act
                var response = await client.GetAsync($"api/v1/channels?channelType={channelType}");

                
                // Assert
                var content = await response.Content.ReadAsStringAsync();
                Assert.True(Regex.IsMatch(
                    content, 
                    "message\\\":.*\"", 
                    RegexOptions.IgnoreCase
                ));
                Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            }
        }
    }
}
