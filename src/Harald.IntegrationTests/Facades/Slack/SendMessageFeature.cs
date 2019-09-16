using System;
using System.Threading.Tasks;
using Harald.IntegrationTests.Util;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Harald.WebApi.Infrastructure.Serialization;
using Xunit;

namespace Harald.IntegrationTests.Facades.Slack
{
    public class SendMessageFeature
    {
        private ISlackFacade _slackFacade;
        private CapabilityCreatedData _capabilityCreatedData;
        
        [Fact]
        public async Task SendMessageFeatureTest()
        {
            Gherkin.RunAsync(
                Given_ASlackTokenAndNewCapability,
                When_CapabilityIsCreated,
                Then_ChannelHasBeenCreatedAndMessageSent
            );
        }

        [Fact]
        public async Task SendMessageFeatureTest2()
        {
            await Gherkin2Builder.New()
                .Given(SlackTokenAndNewCapability)
                .When(CapabilityIsCreated)
                .Then(ChannelHasBeenCreatedAndMessageSent)
                .Build()
                .RunAsync(); 
        }

        private async Task SlackTokenAndNewCapability()
        {
            _slackFacade = new SlackFacade(
                client: SlackFacadeTest.GetHttpClient(),
                serializer: new JsonSerializer()
                );
            _capabilityCreatedData = new CapabilityCreatedData(Guid.NewGuid(), "hellopelle");
        }
        
        private async Task CapabilityIsCreated()
        {
            
        }
        
        private async Task ChannelHasBeenCreatedAndMessageSent()
        {
            var createChannelResponse = await _slackFacade.CreateChannel(
                ChannelName.Create(_capabilityCreatedData.CapabilityName)
            );
            
            var channelName = createChannelResponse?.Channel?.Name;
            if (createChannelResponse.Ok)
            {
                var channelId = new ChannelId(createChannelResponse?.Channel?.Id);

                var sendNotificationResponse = await _slackFacade.SendNotificationToChannel(
                    channelId: channelId,
                    message:
                    $"Thank you for creating capability '{_capabilityCreatedData.CapabilityName}'.\n" +
                    $"This channel has been created in connection with the SendMessageFeature integration test.\n");
            }
        }
        
        private async Task When_CapabilityIsCreated()
        {
            await CapabilityIsCreated();
        }

        private async Task Then_ChannelHasBeenCreatedAndMessageSent()
        {
            await ChannelHasBeenCreatedAndMessageSent();
        }
        
        private async Task Given_ASlackTokenAndNewCapability()
        {
            await SlackTokenAndNewCapability();
        }
        
    }
}