using System;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure;
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
            await Then_given_capability_slack_channels_are_returned(connections);
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


            return capabilityId;
        }

        private async Task<dynamic> When_capability_connections_are_requested(Guid capabilityId)
        {
            var connections = await ApiClient.Connections.GetAsync(
                senderType:"capability", 
                senderId: capabilityId.ToString()
            );

            return connections;
        }

        private async Task Then_given_capability_slack_channels_are_returned(dynamic connections)
        {
            throw new System.NotImplementedException();
        }
    }
}