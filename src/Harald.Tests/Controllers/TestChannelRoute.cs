using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Harald.Tests.Builders;
using Harald.Tests.TestDoubles;
using Harald.WebApi.Domain;
using Harald.Infrastructure.Slack;
using Xunit;
using Harald.WebApi.Infrastructure.Serialization;

namespace Harald.Tests.Controllers
{
    public class TestChannelRoute
    {
        [Fact]
        public async Task Leave_channel_returns_expected_status_code()
        {
            var serializer = new JsonSerializer();
            var capabilityId = Guid.NewGuid();

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ICapabilityRepository>(new StubCapabilityRepository(new List<Guid> { capabilityId }))
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                var channelId = "123FooBar";

                var payload = serializer.GetPayload(new { CapabilityId = capabilityId, ChannelId = channelId });
                var response = await client.PostAsync("api/v1/channel/leave", payload);

                Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            }
        }

        [Fact]
        public async Task Leave_channel_given_empty_capabilityId_returns_expected_status_code()
        {
            var serializer = new JsonSerializer();

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ICapabilityRepository>(new StubCapabilityRepository(new List<Guid> { }))
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                var channelId = "123FooBar";

                var payload = serializer.GetPayload(new { ChannelId = channelId });
                var response = await client.PostAsync("api/v1/channel/leave", payload);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task Leave_channel_given_empty_channelId_returns_expected_status_code()
        {
            var serializer = new JsonSerializer();
            var capabilityId = Guid.NewGuid();

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ICapabilityRepository>(new StubCapabilityRepository(new List<Guid> { capabilityId }))
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                var channelId = string.Empty;

                var payload = serializer.GetPayload(new { CapabilityId = capabilityId, ChannelId = channelId });
                var response = await client.PostAsync("api/v1/channel/leave", payload);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task Leave_channel_given_non_existing_capabilityId_returns_expected_status_code()
        {
            var serializer = new JsonSerializer();
            var capabilityId = Guid.NewGuid();

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ICapabilityRepository>(new StubCapabilityRepository(new List<Guid> { capabilityId }))
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                var nonExistingCapabilityId = Guid.NewGuid();
                var channelId = "123FooBar";

                var payload = serializer.GetPayload(new { CapabilityId = nonExistingCapabilityId, ChannelId = channelId });
                var response = await client.PostAsync("api/v1/channel/leave", payload);

                Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            }
        }
    }
}
