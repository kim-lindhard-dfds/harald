using Harald.Infrastructure.Slack;
using Harald.Tests.Builders;
using Harald.Tests.TestDoubles;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Harald.Tests.Controllers
{
    public class TestChannelsRoute
    {
        [Fact]
        public async Task Get_channels_returns_expected_status_code()
        {
            var channelType = "slack";

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                var response = await client.GetAsync($"api/v1/channels?channelType={channelType}");
                var result = await response.Content.ReadAsStringAsync();

                Assert.True(!string.IsNullOrEmpty(result));
                Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            }
        }
    }
}
