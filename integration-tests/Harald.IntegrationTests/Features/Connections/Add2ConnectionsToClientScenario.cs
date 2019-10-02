using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure;
using Harald.IntegrationTests.Features.Infrastructure.Model;
using Xunit;

namespace Harald.IntegrationTests.Features.Connections
{
    public class Add2ConnectionsToClientScenario
    {
        private SlackApiServerSpyClient slackApiServerSpyClient = new SlackApiServerSpyClient();
        
        private Guid _capabilityId;
        private SlackConversationDto _conversation1Dto;
        private SlackConversationDto _conversation2Dto;
        
        [Fact]
        public async Task Add2ConnectionsToClientScenarioRecipe()
        {
            await Given_a_clean_slack_api_server_spy();
            await and_a_capability();
            await and_a_slack_channel_named_channel1();
            await and_a_slack_channel_named_channel2();
            await When_connection_is_added();
            await And_connection_is_added();
            await Then_3_connection_should_be_in_the_capability_connections_list();
        }

        private async Task and_a_slack_channel_named_channel1()
        {
            _conversation1Dto = await slackApiServerSpyClient.ChannelsCreateAsync("channel1");
        }
        private async Task and_a_slack_channel_named_channel2()
        {
            _conversation2Dto = await slackApiServerSpyClient.ChannelsCreateAsync("channel2");
        }
        private async Task Given_a_clean_slack_api_server_spy()
        {
            await slackApiServerSpyClient.ResetAsync();
        }
        private async Task and_a_capability()
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

            _capabilityId = capabilityId;
        }

        private async Task When_connection_is_added()
        {
            await ApiClient.Connections.AddAsync(
                "VerbNoun",
                _capabilityId.ToString(),
                _conversation1Dto.Name,
                _conversation1Dto.Id
            );
        }

        private async Task And_connection_is_added()
        {
            await ApiClient.Connections.AddAsync(
                "VerbNoun",
                _capabilityId.ToString(),
                _conversation2Dto.Name,
                _conversation2Dto.Id
            );
        }

        private async Task Then_3_connection_should_be_in_the_capability_connections_list()
        {
            var connectionsEnvelope = await ApiClient.Connections.GetAsync(
                senderType: "capability",
                senderId: _capabilityId.ToString()
            );

            var connections = connectionsEnvelope.Items;

            connections.Single(c => c.ChannelName == "verbnoun");
            connections.Single(c => c.ChannelName == "channel1");
            connections.Single(c => c.ChannelName == "channel2");
        }
    }
}