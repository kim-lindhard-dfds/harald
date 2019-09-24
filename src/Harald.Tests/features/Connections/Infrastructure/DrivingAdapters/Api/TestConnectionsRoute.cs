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

namespace Harald.Tests.Features.Connections.Infrastructure.DrivingAdapters.Api
{
    public class TestConnectionsRoute
    {
        [Fact]
        public async Task delete_connection_returns_expected_status_code()
        {
            var clientId = Guid.NewGuid();
            var clientType = "capability";

            using (var builder = new HttpClientBuilder())
            {
                var client = builder
                    .WithService<ICapabilityRepository>(new StubCapabilityRepository(new List<Guid> { clientId }))
                    .WithService<ISlackFacade>(new SlackFacadeStub(simulateFailOnSendMessage: false))
                    .Build();

                var response = await client.DeleteAsync($"api/v1/connections?clientId={clientId}&clientType={clientType}");

                Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            }
        }

        [Fact]
        public async Task delete_connection_given_empty_clientId_returns_expected_status_code()
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
                var response = await client.PostAsync("api/v1/connections", payload);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task delete_connection_given_empty_channelId_returns_expected_status_code()
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
    }
}
