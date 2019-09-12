using Xunit;

namespace Harald.IntegrationTests.Facades.Slack
{
    public class CapabilityCreatedScenario
    {
        [Fact]
        public void CapabilityCreatedScenarioRecipe()
        {
            When_a_capability_is_created();
            Then_a_slack_channel_is_created();
            Then_a_Notification_is_sent_to_channel();
            Then_the_notification_is_pinned_to_the_channel();
        }
        
        
        public static void When_a_capability_is_created(){}
        public static void Then_a_slack_channel_is_created(){}
        public static void Then_a_Notification_is_sent_to_channel(){}
        public static void Then_the_notification_is_pinned_to_the_channel(){}
    }
}