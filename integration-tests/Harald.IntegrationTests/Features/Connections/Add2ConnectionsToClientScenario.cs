using System;
using System.Threading;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure;
using Xunit;

namespace Harald.IntegrationTests.Features.Connections
{
    public class Add2ConnectionsToClientScenario
    {
        [Fact]
        public async Task Add2ConnectionsToClientScenarioRecipe()
        {
            var capabilityId = await Given_a_capability();
            await When_connection_is_added(capabilityId);
            await And_connection_is_added(capabilityId);
            await Then_3_connection_should_be_in_the_capability_connections_list(capabilityId);
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

        private async Task When_connection_is_added(Guid capabilityId)
        {
            await ApiClient.Connections.AddAsync(
                "VerbNoun",
                capabilityId.ToString(),
                "channel1",
                "HJDPS78LS"
            );
        }

        private async Task And_connection_is_added(Guid capabilityId)
        {
            await ApiClient.Connections.AddAsync(
                "VerbNoun",
                capabilityId.ToString(),
                "channel2",
                "AALF173LF"
            );
        }

        private async Task Then_3_connection_should_be_in_the_capability_connections_list(Guid capabilityId)
        {
            var connectionsEnvelope = await ApiClient.Connections.GetAsync(
                senderType: "capability",
                senderId: capabilityId.ToString()
            );

            var connections = connectionsEnvelope.Items;

            Assert.Single(connections, c => c.ClientName == "VerbNoun");
            Assert.Single(connections, c => c.ClientName == "channel1");
            Assert.Single(connections, c => c.ClientName == "channel2");
        }
    }
}