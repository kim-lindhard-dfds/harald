using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure;
using Harald.IntegrationTests.Features.Infrastructure.Model;
using Xunit;

namespace Harald.IntegrationTests.Features.Channels
{
    public class ListCapabilityConnectionsScenario
    {
        [Fact]
        public async Task ListCapabilityConnectionsScenarioRecipe()
        {
            var capabilityId = await Given_a_capability();
            var connections = await When_capability_connections_are_requested(capabilityId);
            await Then_given_capability_slack_channels_are_returned(capabilityId, connections);
        }

        private async Task<Guid> Given_a_capability()
        {
            var capabilityId = Guid.NewGuid();
            var kafkaClient = new KafkaClient();

            var message = new
            {
                CapabilityId = capabilityId.ToString(),
                CapabilityName = "VerbNoun",
            };
            await kafkaClient.SendMessageAsync(message);

            Thread.Sleep(1500); // Magic sleep that ensures that the service have complected the create operation. 

            return capabilityId;
        }

        private async Task<IEnumerable<ConnectionDto>> When_capability_connections_are_requested(Guid capabilityId)
        {
            var connections = await ApiClient.Connections.GetAsync(
                senderType: "capability",
                senderId: capabilityId.ToString()
            );

            return connections;
        }

        private async Task Then_given_capability_slack_channels_are_returned(
            Guid capabilityId,
            IEnumerable<ConnectionDto> connections
        )
        {
            Assert.True(connections.Any());
            Assert.All(connections, dto => { Assert.Equal(capabilityId.ToString(), dto.SenderId); });
            // TODO Ensure the connections point to the correct channels
        }
    }
}