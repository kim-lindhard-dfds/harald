using System;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure;
using Xunit;

namespace Harald.IntegrationTests.Features.ChannelManagement
{
    public class CapabilityCreatedScenario
    {
        private SlackApiServerSpyClient slackApiServerSpyClient = new SlackApiServerSpyClient();

        [Fact]
        public async Task CapabilityCreatedScenarioRecipe()
        {
            await Given_a_clean_slack_api_server_spy();
            await When_a_capability_is_created();
            await Then_a_slack_channel_is_created();
            await Then_a_slack_user_group_is_created();
            await Then_a_Notification_is_sent_to_channel();
            await Then_the_notification_is_pinned_to_the_channel();
        }

        private async Task Given_a_clean_slack_api_server_spy()
        {
            await slackApiServerSpyClient.ResetAsync();
        }


        private async Task When_a_capability_is_created()
        {
            var kafkaClient = new KafkaClient();

            var message = new
            {
                CapabilityId = Guid.NewGuid().ToString(),
                CapabilityName = "VerbNoun",
            };
            await kafkaClient.SendMessageAsync(message);
        }

        
        private async Task Then_a_slack_channel_is_created()
        {
            var interaction = await slackApiServerSpyClient.GetNextInteractionWith5SecRetryAsync();

            Assert.Equal("/api/channels.create", interaction.path.ToString());
        }

        
        private async Task Then_a_slack_user_group_is_created()
        {
            var interaction = await slackApiServerSpyClient.GetNextInteractionWith5SecRetryAsync();

            Assert.Equal("/api/usergroups.list", interaction.path.ToString());

            interaction = await slackApiServerSpyClient.GetNextInteractionWith5SecRetryAsync();

            Assert.Equal("/api/usergroups.create", interaction.path.ToString());
        }

        
        private async Task Then_a_Notification_is_sent_to_channel()
        {
            var interaction = await slackApiServerSpyClient.GetNextInteractionWith5SecRetryAsync();

            Assert.Equal("/api/chat.postMessage", interaction.path.ToString());
        }

        
        private async Task Then_the_notification_is_pinned_to_the_channel()
        {
            var interaction = await slackApiServerSpyClient.GetNextInteractionWith5SecRetryAsync();

            Assert.Equal("/api/pins.add", interaction.path.ToString());
        }
    }
}