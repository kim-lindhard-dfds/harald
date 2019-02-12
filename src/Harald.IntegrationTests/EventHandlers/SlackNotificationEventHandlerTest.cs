using System;
using System.Net.Http;
using Xunit;
using Harald.Application.EventHandlers;
using Harald.Domain.Capability.Events;
using Harald.Infrastructure.Facades.Slack;
using Harald.Infrastructure.Serialization;
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
            var authToken = Environment.GetEnvironmentVariable("SLACK_API_AUTH_TOKEN");

            httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {authToken}");

            var slackFacade = new SlackFacade(httpClient, new JsonSerializer());
            var sut = new SlackNotificationEventHandler(slackFacade);
            var recipient = "janie-test";//"ded-team-one";
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