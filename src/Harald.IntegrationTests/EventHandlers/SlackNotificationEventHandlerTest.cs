using System;
using System.Net.Http;
using Xunit;
using Harald.Application.EventHandlers;
using Harald.Domain.Capability.Events;
using Harald.Infrastructure.Facades;
using System.Net;
using System.Threading.Tasks;

namespace Harald.IntegrationTests.EventHandlers
{
    public class SlackNotificationEventHandlerTest
    {
        [Fact]
        public async Task Handle_Given_valid_input_Should_trigger_notification()
        {
            // Arrange
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://slack.com");
            var botToken = Environment.GetEnvironmentVariable("SLACK_API_BOT_TOKEN");

            httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {botToken}");

            var slackFacade = new SlackFacade(httpClient);
            var sut = new SlackNotificationEventHandler(slackFacade);
            var recipient = "ded-team-one";
            var message = "Test via facade.";
            var domainEvent = new SendNotificationDomainEvent(
                aggregateRootId: Guid.NewGuid(),
                version: 1,
                recipient: recipient,
                message: message);

            // Act
            await sut.HandleAsync(domainEvent);

            // Assert
        }
    }
}
