using System;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure;
using Xunit;

namespace Harald.IntegrationTests.Features.ChannelManagement
{
    public class CapabilityCreatedScenario
    {
        [Fact]
        public async Task CapabilityCreatedScenarioRecipe()
        {
            await When_a_capability_is_created();
            Then_a_slack_channel_is_created();
            Then_a_Notification_is_sent_to_channel();
            Then_the_notification_is_pinned_to_the_channel();
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

        public static void Then_a_slack_channel_is_created()
        {
        }

        public static void Then_a_Notification_is_sent_to_channel()
        {
        }

        public static void Then_the_notification_is_pinned_to_the_channel()
        {
        }
    }
}